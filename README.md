# Inside Langsam 411: A Virtual Reality Recreation of University of Cincinnati's Classroom

## Motivation
We wanted to challenge the idea that learning spaces have to feel formal or stressful. Our goal was to design a virtual classroom that prioritizes comfort, creativity, and playfulness, showing how VR can transform an everyday environment into something more personal and inviting.


## Design
**Room Overview:**
We revamped our classroom into a stress free learning space with warm colors, comfy seating, and fun gaming elements. It's designed to feel welcoming and flexible which is perfect for studying solo or hanging out with others.

**Visual Elements:**
The room has comfortable bean bags and chairs, arcade games, a turntable with vinyl record playing 70's music, lava lamp, plants, and everyday objects like coffee cups and laptops. The soft, warm lighting keeps the vibe relaxed, calm, and inviting.

**How We Built It:**
We grabbed assets from Meshy.AI for the fun and more unique stuff (pinball, bean bags), CGTrader for props and furniture, and AvatarSDK for custom avatars. This let us fill the space with quality assets that all looked cohesive.

## Accomplishments by Level
**Level 1:** Furnished the room with comfortable seating (couches & bean bags), warm lighting, and ambient sounds (pinball, laptop fan sound, music playing from the turntable).

**Level 2:** Added interactive objects (coffee cups, laptops, notebooks, pencils, plants) with physics and collision detection.

**Level 3:** Added four custom avatars of ourselves with idle animations (characters dancing) and interactions (they wave as you walk up to them). The large display has a static image but it also has a moving screensaver as you walk up to it.

**Level 3 Bonus:** Created custom avatar characters that look like us using AvatarSDK.com.

**Level 4:** Approaching the pinball machine plays sound, the laptop opens as you near it, the turntable's music grows louder or softer with your distance, and walking up to the avatars makes them stop dancing and wave.

**Level 5:** Added a spinning disco ball with fun lighting effects.

TODO: Add Screenshots

**Credits**
- **[Meshy.AI](https://www.meshy.ai):** Orange bean bag, pinball, guitar, turntable
- **[CGTrader](https://www.cgtrader.com):** Lava lamp, pencil, notebook, coffee cup, gym bag, plants
- **[Unity Asset Store](https://assetstore.unity.com/):** Tables and chairs
- **[AvatarSDK.com](https://avatarsdk.com):** Custom characters
- **[Mixamo.com](https://www.mixamo.com/#/):** Character Animations

**Sounds:**
- **Pinball:** https://youtu.be/gdjx1Dbxpro?si=X661oeg6W-eNQMAY
- **Dancing Queen:** https://youtu.be/h3KJD9G80dc?si=JisPuD0CwySlWxZ6
- **Computer Fan:** https://youtu.be/5__04FsiUFc?si=mj-lYYvPO7gbFx7f

## Process
- Clone & open in Unity 6.3 LTS (6000.3.1f1)
- Load `SampleScene.unity` and press Play
- Main scripts: `AvatarRoomWanderer.cs`, `MixamoMotionModelImporter.cs`
- Tech: Unity, C#, [Mixamo](https://www.mixamo.com), [Meta Quest 3](https://www.meta.com/quest/quest-3)

[Add in how we made this application. How did you structure your code?  How can you access it and run it?  What libraries or API's did you use?]
 
## Challenges & Future Work
- It was hard to connect MetaQuest to Unity because there was a lot of bugs when installing, setting it up, and dealing with storage issues.
- Some computers were more compatible than others, and had an easier time rendering and connecting the environment to Unity. 
- A teammember couldn't work with the simulator due to having MacOS Intel.
- We initially struggled with connecting the MetaQuest controllers to Unity. 
- We had a difficult time asynchronously working on the project since there wasn't an efficient way to share work or have a smooth version control system setup when it comes to Unity, and it slowed down development efficiency and put burden on one of the team members more than the others which felt unfair.
- As we neared the end of the project and we had a ton of assets in Unity that made our programs crash continious and we had a lot of trouble getting the controllers to work.
- In the future we'd continue adding more unique assets and add more AR elements to make the scenes more realistic and fun to interact with.

## AI & Collaboration
Minimal AI usage besides for asset creation - mostly developed manually.

## Demo Video
[Link to demo](https://drive.google.com/file/d/1CcVdJR8fiT5L79GzELgUpgX1q9ONDaav/view?usp=sharing)

