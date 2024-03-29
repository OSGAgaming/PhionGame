﻿namespace QueefCord.Core.Resources.Loaders
{
    /// <summary>
    /// Implementing this interface allows your class to be used as a loader for content by the AssetServer.
    /// </summary>
    public interface ILoader
    {
        /// <summary>
        /// The query your loader searches for inside the content folder.
        /// </summary>
        string FileQuery { get; }

        /// <summary>
        /// Called whenever a file that matches the query needs to be loaded.
        /// </summary>
        /// <param name="path">The full path to the file.</param>
        void Load(string path);

    }
}
