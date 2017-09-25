using System.Linq;
using UnityEditor;
using UnityEngine;
using Uplift.Common;
using Uplift.DependencyResolution;
using Uplift.Packages;
using Uplift.Schemas;

namespace Uplift.Windows
{
    internal class UpdateUtility : EditorWindow
    {
        private Vector2 scrollPosition;
        private UpliftManager manager;
        private Upbring upbring;
        private Upfile upfile;
        private PackageList packageList;
        private PackageRepo[] packageRepos;

        protected void OnGUI()
        {
            titleContent.text = "Update Utility";

            manager = UpliftManager.Instance();
            upbring = Upbring.Instance();
            upfile = Upfile.Instance();
            packageList = PackageList.Instance();
            packageRepos = packageList.GetAllPackages();

            IDependencySolver solver = manager.GetDependencySolver();
            DependencyDefinition[] dependencies = new DependencyDefinition[0];

            try
            {
                dependencies = solver.SolveDependencies(upfile.Dependencies);
                DependencyDefinition[] directDependencies = new DependencyDefinition[upfile.Dependencies.Length];
                for(int i = 0; i < upfile.Dependencies.Length; i++)
                {
                    directDependencies[i] = dependencies.First(dep => dep.Name == upfile.Dependencies[i].Name);
                }
                DependencyDefinition[] transitiveDependencies = dependencies.Except(directDependencies).ToArray();

                bool any_installed =
                        upbring.InstalledPackage != null &&
                        upbring.InstalledPackage.Length != 0;

                if (directDependencies.Length == 0)
                {
                    EditorGUILayout.HelpBox("It seems that you didn't specify any dependency in the Upfile. Try refreshing it if you did.", MessageType.Warning);
                }
                else
                {
                    scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
                    EditorGUILayout.LabelField("Direct dependencies:", EditorStyles.boldLabel);
                    EditorGUILayout.Space();

                    foreach (DependencyDefinition dependency in directDependencies)
                        DependencyBlock(dependency, any_installed);

                    if(transitiveDependencies.Length != 0)
                    {
                        EditorGUILayout.Space();
                        EditorGUILayout.LabelField("Transitive dependencies:", EditorStyles.boldLabel);
                        EditorGUILayout.Space();

                        foreach (DependencyDefinition dependency in transitiveDependencies)
                            DependencyBlock(dependency, any_installed);
                    }

                    EditorGUILayout.EndScrollView();

                    if (GUILayout.Button("Install all dependencies"))
                    {
                        manager.InstallDependencies();

                        AssetDatabase.Refresh();

                        Repaint();
                    }
                    GUI.enabled = any_installed;
                    if (GUILayout.Button("Update all installed packages"))
                    {
                        foreach (InstalledPackage package in upbring.InstalledPackage)
                        {
                            PackageRepo latestPackageRepo = packageList.GetLatestPackage(package.Name);
                            if (package.Version != latestPackageRepo.Package.PackageVersion)
                            {
                                Debug.Log(string.Format("Updating package {0} (to {1})", package.Name, latestPackageRepo.Package.PackageVersion));
                                manager.UpdatePackage(latestPackageRepo);
                            }
                        }

                        AssetDatabase.Refresh();

                        Repaint();
                    }
                    GUI.enabled = true;
                    if (GUILayout.Button("Refresh Upfile"))
                    {
                        UpliftManager.ResetInstances();

                        Repaint();
                    }
                }
            }
            catch(IncompatibleRequirementException e)
            {
                EditorGUILayout.HelpBox("There is a conflict in your dependency tree:\n" + e.ToString(), MessageType.Error);
            }
            catch(MissingDependencyException e)
            {
                EditorGUILayout.HelpBox("A dependency cannot be found in any of your specified repository:\n" + e.ToString(), MessageType.Error);
            }
        }

        private void DependencyBlock(DependencyDefinition definition, bool any_installed)
        {
            string name = definition.Name;
            EditorGUILayout.LabelField(name + ":", EditorStyles.boldLabel);
            EditorGUILayout.LabelField("Requirement: " + definition.Requirement.ToString());
            bool installable = packageRepos.Any(pr => pr.Package.PackageName == name);
            bool installed =
                any_installed &&
                upbring.InstalledPackage.Any(ip => ip.Name == name);
            string installed_version = installed ? upbring.GetInstalledPackage(name).Version : "";

            if (installed)
            {
                EditorGUILayout.LabelField("- Installed version is " + installed_version);
            }
            else
            {
                EditorGUILayout.LabelField("- Not yet installed");
            }

            if (!installable)
            {
                EditorGUILayout.HelpBox("No repository contains this package. Try specifying one whith this package in.", MessageType.Warning);
            }
            else
            {
                PackageRepo latestPackageRepo = packageList.GetLatestPackage(name);
                string latest_version = latestPackageRepo.Package.PackageVersion;

                EditorGUILayout.LabelField(string.Format("- Latest version is: {0} (from {1})", latest_version, latestPackageRepo.Repository.ToString()));
                if (!definition.Requirement.IsMetBy(latest_version))
                {
                    EditorGUILayout.HelpBox("The latest available version does not meet the requirement of the dependency.", MessageType.Warning);
                }

                GUI.enabled = installed && installed_version != latest_version;
                if (GUILayout.Button("Update to " + latest_version))
                {
                    Debug.Log(string.Format("Updating package {0} (to {1})", name, latest_version));
                    manager.UpdatePackage(latestPackageRepo);

                    AssetDatabase.Refresh();

                    Repaint();
                }
                GUI.enabled = true;
            }

            EditorGUILayout.Space();
        }
    }
}