using System.Collections;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;
#if UNITY_EDITOR
using UnityEditor;
#endif

[RequireComponent(typeof(Animator))]
public sealed class AvatarRoomWanderer : MonoBehaviour
{
    private const string MixamoMotionFolder = "Assets/Avatars/movements/mixamo motions/";

    [Header("Room")]
    [SerializeField] private Transform roomRoot;
    [SerializeField] private string roomName = "Langsam Room";
    [SerializeField] private float wallPadding = 0.65f;

    [Header("Motion Clips")]
    [SerializeField] private AnimationClip[] idleClips;
    [SerializeField] private AnimationClip[] walkClips;

    [Header("Timing")]
    [SerializeField] private Vector2 idleTimeRange = new Vector2(2.5f, 5.5f);
    [SerializeField] private Vector2 walkDistanceRange = new Vector2(1.2f, 3.5f);
    [SerializeField] private float walkSpeed = 0.85f;
    [SerializeField] private float turnSpeed = 360f;
    [SerializeField] private float clipFadeDuration = 0.25f;

    private Animator animator;
    private PlayableGraph graph;
    private AnimationMixerPlayable mixer;
    private AnimationClipPlayable currentPlayable;
    private AnimationClipPlayable nextPlayable;
    private bool hasCurrentPlayable;
    private int currentInput;
    private Bounds roomBounds;
    private float fixedY;

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (idleClips == null || idleClips.Length == 0 || HasMissingClip(idleClips))
        {
            idleClips = LoadClips(
                "Idle",
                "Standing Idle 03",
                "Catwalk Idle To Twist R",
                "Dismissing Gesture",
                "Nervously Look Around",
                "Salute");
        }

        if (walkClips == null || walkClips.Length == 0 || HasMissingClip(walkClips))
        {
            walkClips = LoadClips("Start Walking", "Walk (1)", "Strut Walking");
        }
    }
#endif

    private void Awake()
    {
        animator = GetComponent<Animator>();
        animator.applyRootMotion = false;
        fixedY = transform.position.y;
        ResolveRoomBounds();
        CreateGraph();
    }

    private void OnEnable()
    {
        if (graph.IsValid())
        {
            graph.Play();
        }

        StartCoroutine(WanderLoop());
    }

    private void OnDisable()
    {
        StopAllCoroutines();

        if (graph.IsValid())
        {
            graph.Stop();
        }
    }

    private void OnDestroy()
    {
        if (graph.IsValid())
        {
            graph.Destroy();
        }
    }

    private IEnumerator WanderLoop()
    {
        ClampToRoom();

        while (enabled)
        {
            var idleClip = PickClip(idleClips);
            if (idleClip != null)
            {
                yield return PlayClipForSeconds(idleClip, Random.Range(idleTimeRange.x, idleTimeRange.y));
            }

            var walkClip = PickClip(walkClips);
            if (walkClip != null)
            {
                yield return WalkTo(RandomPointInRoom(), walkClip);
            }
            else
            {
                yield return null;
            }
        }
    }

    private IEnumerator WalkTo(Vector3 target, AnimationClip clip)
    {
        PlayClip(clip);

        while (Vector3.Distance(FlatPosition(transform.position), FlatPosition(target)) > 0.08f)
        {
            var position = transform.position;
            var toTarget = FlatPosition(target) - FlatPosition(position);

            if (toTarget.sqrMagnitude > 0.001f)
            {
                var targetRotation = Quaternion.LookRotation(toTarget.normalized, Vector3.up);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
            }

            var step = Mathf.Min(walkSpeed * Time.deltaTime, toTarget.magnitude);
            transform.position = ClampToRoom(position + toTarget.normalized * step);
            yield return null;
        }
    }

    private IEnumerator PlayClipForSeconds(AnimationClip clip, float seconds)
    {
        PlayClip(clip);

        var endTime = Time.time + Mathf.Max(0.1f, seconds);
        while (Time.time < endTime)
        {
            ClampToRoom();
            yield return null;
        }
    }

    private void PlayClip(AnimationClip clip)
    {
        if (clip == null)
        {
            return;
        }

        clip.wrapMode = WrapMode.Loop;

        var newInput = 1 - currentInput;
        nextPlayable = AnimationClipPlayable.Create(graph, clip);
        nextPlayable.SetApplyFootIK(true);
        nextPlayable.SetTime(0);

        mixer.DisconnectInput(newInput);
        mixer.ConnectInput(newInput, nextPlayable, 0);
        StartCoroutine(FadeToInput(newInput));
    }

    private IEnumerator FadeToInput(int newInput)
    {
        var oldInput = currentInput;
        var oldPlayable = currentPlayable;
        var duration = Mathf.Max(0.01f, clipFadeDuration);
        var elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            var t = Mathf.Clamp01(elapsed / duration);
            mixer.SetInputWeight(oldInput, hasCurrentPlayable ? 1f - t : 0f);
            mixer.SetInputWeight(newInput, t);
            yield return null;
        }

        mixer.SetInputWeight(oldInput, 0f);
        mixer.SetInputWeight(newInput, 1f);

        if (hasCurrentPlayable && oldPlayable.IsValid())
        {
            oldPlayable.Destroy();
        }

        currentInput = newInput;
        currentPlayable = nextPlayable;
        hasCurrentPlayable = true;
    }

    private void CreateGraph()
    {
        graph = PlayableGraph.Create($"{name} Room Wander");
        graph.SetTimeUpdateMode(DirectorUpdateMode.GameTime);

        mixer = AnimationMixerPlayable.Create(graph, 2);
        var output = AnimationPlayableOutput.Create(graph, "Animation", animator);
        output.SetSourcePlayable(mixer);

        graph.Play();
    }

    private void ResolveRoomBounds()
    {
        if (roomRoot == null)
        {
            var room = GameObject.Find(roomName) ?? GameObject.Find(roomName.Trim()) ?? FindByTrimmedName(roomName);
            if (room != null)
            {
                roomRoot = room.transform;
            }
        }

        if (roomRoot == null || !TryGetRendererBounds(roomRoot, out roomBounds))
        {
            roomBounds = new Bounds(transform.position, new Vector3(6f, 3f, 6f));
        }

        var size = roomBounds.size;
        size.x = Mathf.Max(0.2f, size.x - wallPadding * 2f);
        size.z = Mathf.Max(0.2f, size.z - wallPadding * 2f);
        roomBounds.size = size;
    }

    private static bool TryGetRendererBounds(Transform root, out Bounds bounds)
    {
        var renderers = root.GetComponentsInChildren<Renderer>();
        if (renderers.Length == 0)
        {
            bounds = default;
            return false;
        }

        bounds = renderers[0].bounds;
        for (var i = 1; i < renderers.Length; i++)
        {
            bounds.Encapsulate(renderers[i].bounds);
        }

        return true;
    }

    private Vector3 RandomPointInRoom()
    {
        var origin = ClampToRoom(transform.position);
        var desiredDistance = Random.Range(walkDistanceRange.x, walkDistanceRange.y);

        for (var i = 0; i < 12; i++)
        {
            var direction = Random.insideUnitCircle.normalized;
            var candidate = origin + new Vector3(direction.x, 0f, direction.y) * desiredDistance;
            candidate.y = fixedY;

            if (ContainsXZ(candidate))
            {
                return candidate;
            }
        }

        return new Vector3(
            Random.Range(roomBounds.min.x, roomBounds.max.x),
            fixedY,
            Random.Range(roomBounds.min.z, roomBounds.max.z));
    }

    private Vector3 ClampToRoom()
    {
        var clamped = ClampToRoom(transform.position);
        transform.position = clamped;
        return clamped;
    }

    private Vector3 ClampToRoom(Vector3 position)
    {
        position.x = Mathf.Clamp(position.x, roomBounds.min.x, roomBounds.max.x);
        position.y = fixedY;
        position.z = Mathf.Clamp(position.z, roomBounds.min.z, roomBounds.max.z);
        return position;
    }

    private bool ContainsXZ(Vector3 position)
    {
        return position.x >= roomBounds.min.x &&
            position.x <= roomBounds.max.x &&
            position.z >= roomBounds.min.z &&
            position.z <= roomBounds.max.z;
    }

    private static Vector3 FlatPosition(Vector3 position)
    {
        position.y = 0f;
        return position;
    }

    private static AnimationClip PickClip(AnimationClip[] clips)
    {
        if (clips == null || clips.Length == 0)
        {
            return null;
        }

        return clips[Random.Range(0, clips.Length)];
    }

#if UNITY_EDITOR
    private static AnimationClip[] LoadClips(params string[] clipNames)
    {
        var clips = new AnimationClip[clipNames.Length];

        for (var i = 0; i < clipNames.Length; i++)
        {
            clips[i] = LoadClip($"{MixamoMotionFolder}{clipNames[i]}.fbx");
        }

        return clips;
    }

    private static AnimationClip LoadClip(string path)
    {
        foreach (var asset in AssetDatabase.LoadAllAssetsAtPath(path))
        {
            if (asset is AnimationClip clip && !clip.name.StartsWith("__preview__", System.StringComparison.Ordinal))
            {
                return clip;
            }
        }

        return null;
    }

    private static bool HasMissingClip(AnimationClip[] clips)
    {
        foreach (var clip in clips)
        {
            if (clip == null)
            {
                return true;
            }
        }

        return false;
    }
#endif

    private static GameObject FindByTrimmedName(string targetName)
    {
        var trimmedTarget = targetName.Trim();
        var transforms = FindObjectsByType<Transform>(FindObjectsSortMode.None);

        foreach (var candidate in transforms)
        {
            if (candidate.name.Trim() == trimmedTarget)
            {
                return candidate.gameObject;
            }
        }

        return null;
    }
}
