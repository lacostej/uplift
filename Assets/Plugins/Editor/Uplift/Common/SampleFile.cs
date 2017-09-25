﻿using System;
using System.IO;
using System.Text;
using System.Xml;
using UnityEngine;

namespace Uplift.Common
{
    public class SampleFile
    {
        internal static readonly string upfile = @"<Upfile>
  <UnityVersion>{0}</UnityVersion>   
  <!-- SAMPLE REPOSITORIES BLOCK

  <Repositories>
    <FileRepository Path=""Path/To/Some/File/Repository"" />
  </Repositories>

  -->

  <Repositories>
  </Repositories>
    
  <Configuration>
    <!--
        Important note:

        We're not tracing Location settings for anything unpacked outside of Assets.
        This means, that if you change those in settings while already
        having packages/other stuff installed then you need to cleanup
        previous locations manually at your own discretion.
    -->
    <!--
        Path Flags:

        SkipPackageStructure (true|false):
          Don't create package subdirectories
          (in form of PackageName~1.0.0)
    -->
  
    <!--
        Repository configuration part.

        Most often, files here shouldn't be visible by your project.
        i.e. should be put out of the Assets/ path in your project.
    -->

    <!-- Path where downloaded, raw, packages are unpacked        -->
    <RepositoryPath  Location=""UPackages""                         />
    <!-- Path where documentation are unpacked                    -->
    <DocsPath        Location=""UPackages/Docs""                    />
    <!-- Path where examples are unpacked                         -->
    <ExamplesPath    Location=""UPackages/Examples""                />

    <!--
        Project configuration part.

        Paths set here most often should be visible by your project,
        i.e.be in Assets path.
    -->

    <!-- Path where ""usable"" files are unpacked                   -->
    <BaseInstallPath    Location=""Assets/UPackages""               />
    <!-- Path where media  files are unpacked                     -->
    <MediaPath          Location=""Assets/UPackages""               />
    <!-- 
        NOTE: the following paths have specific functionnality in Unity.
        Please make sure you know what you're doing before modifying them.
    -->
    <!-- Path where plugin files are unpacked                     -->
    <PluginPath         Location=""Assets/Plugins""                 />
    <!-- Path where editor plugin files are unpacked              -->
    <EditorPluginPath   Location=""Assets/Plugins/Editor""          />
    <!-- Path where gizmos are unpacked                           -->
    <GizmoPath          Location=""Assets/Gizmos""                  />

  </Configuration>

  <Dependencies>

  </Dependencies>

  <!-- SAMPLE DEPENDENCIES BLOCK

  <Dependencies>
    <Package Name=""SomePackage"" Repository=""Optional.Repository"" Version=""Optional.Version.Spec"" />
    <Package Name=""PackageA"">
      <MajorVersionMin>3</MajorVersionMin>
    </Package>
  </Dependencies>

  -->
</Upfile>";

        public static void CreateSampleUpfile()
        {
            XmlDocument sampleFile = new XmlDocument();

            sampleFile.InnerXml = string.Format(upfile, Application.unityVersion);

            sampleFile.Save("Upfile.xml");
        }
    }
}
