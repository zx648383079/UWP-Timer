using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZoDream.LogTimer.Utils
{
    public enum CameraHelperResult
    {
        /// <summary>
        /// Initialization was successful.
        /// </summary>
        Success,

        /// <summary>
        /// Initialization failed; Frame Reader Creation failed.
        /// </summary>
        CreateFrameReaderFailed,

        /// <summary>
        /// Initialization failed; Unable to start Frame Reader.
        /// </summary>
        StartFrameReaderFailed,

        /// <summary>
        /// Initialization failed; Frame Source Group is null.
        /// </summary>
        NoFrameSourceGroupAvailable,

        /// <summary>
        /// Initialization failed; Frame Source is null.
        /// </summary>
        NoFrameSourceAvailable,

        /// <summary>
        /// Access to the camera is denied.
        /// </summary>
        CameraAccessDenied,

        /// <summary>
        /// Initialization failed due to an exception.
        /// </summary>
        InitializationFailed_UnknownError,

        /// <summary>
        /// Initialization failed; No compatible frame format exposed by the frame source.
        /// </summary>
        NoCompatibleFrameFormatAvailable
    }
}
