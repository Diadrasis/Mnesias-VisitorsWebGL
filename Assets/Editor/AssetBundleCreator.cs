using UnityEditor;
//this script is used for the videos to try and minimize their size in order to easily build it for google play. On all other platforms we don't face any problem with size of the project
public class CreateAssetBundles
{
	[MenuItem("Assets/Build AssetBundles")]
	static void BuildAllAssetBundles()
	{
		BuildPipeline.BuildAssetBundles("Assets/AssetBundles", BuildAssetBundleOptions.None, BuildTarget.Android);

	}


}
