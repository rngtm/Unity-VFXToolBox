using System;
using System.Linq;
using System.Threading.Tasks;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.PackageManager;
using UnityEditor.PackageManager.Requests;
using UnityEngine;

namespace VfxToolBox
{
    public static class PackageConfig
    {
        public static readonly string PackageName = "com.rngtm.vfx-toolbox";

        public static string GetPackagePath(string path) => $"Packages/{PackageName}/{path}";
        private static ListRequest Request;
        private static bool isInstallInstallPackage = false;
        public static bool IsInstallPackage => isInstallInstallPackage;


        [MenuItem(VfxMenuConfig.TestRootMenuName + "Check Package Installation")]
        [DidReloadScripts]
        public static void CheckInstall()
        {
            Request = Client.List();
            EditorApplication.update += Progress;
        }

        static void Progress()
        {
            if (Request.IsCompleted)
            {
                if (Request.Status == StatusCode.Success)
                {
                    if (Request.Result.Select(q => q.name).Contains(PackageName))
                    {
                        // Debug.Log($"Is Install : {PackageName}");
                        isInstallInstallPackage = true;
                    }
                    else
                    {
                        // Debug.Log($"Is Not Install : {PackageName}");
                        isInstallInstallPackage = false;
                    }
                    // foreach (var package in Request.Result)
                    //     Debug.Log("Package name: " + package.name);
                }
                else if (Request.Status >= StatusCode.Failure)
                {
                    Debug.Log(Request.Error.message);
                }

                EditorApplication.update -= Progress;
            }
        }
    }
}