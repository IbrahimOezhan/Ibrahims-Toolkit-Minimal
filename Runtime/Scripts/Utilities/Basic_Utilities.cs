using System;
using System.IO;
using UnityEngine;

namespace IbrahKit
{
    public static class Basic_Utilities
    {
        public static void Screenshot()
        {
            string fileName = "Screenshot-" + DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss") + ".png";
            string screenshotsPath = Path.Combine(Path_Utilities.GetGamePath(), "Screenshots");
            if (!Directory.Exists(screenshotsPath)) Directory.CreateDirectory(screenshotsPath);
            ScreenCapture.CaptureScreenshot(Path.Combine(screenshotsPath, fileName));
        }
    }
}