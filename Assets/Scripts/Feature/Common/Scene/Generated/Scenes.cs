using Feature.Interface;

// <auto-generated>
// This code was generated by SceneUtility.
// Do not modify this file manually.
// </auto-generated>

namespace Feature.Common.Scene.Generated
{
    public enum Scene
    {
        SampleScene,
        Robbie,
        Title,
    }

    public static class SceneLoaderFeatures
    {
        public static SceneLoader SampleScene(ISceneDataModel sceneDataModel)
        {
            return new SceneLoader(Scene.SampleScene, "Assets/Scenes/SampleScene.unity", sceneDataModel);
        }
        public static SceneLoader Robbie(ISceneDataModel sceneDataModel)
        {
            return new SceneLoader(Scene.Robbie, "Assets/Scenes/Prod/Game/Robbie.unity", sceneDataModel);
        }
        public static SceneLoader Title(ISceneDataModel sceneDataModel)
        {
            return new SceneLoader(Scene.Title, "Assets/Scenes/Prod/Title/Title.unity", sceneDataModel);
        }
    }
}
