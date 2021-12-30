namespace XperienceCommunity.PageBuilderUtilities
{
    /// <summary>
    /// Abstraction for the current request's Page Builder mode
    /// </summary>
    public interface IPageBuilderContext
    {
        /// <summary>
        /// True if either <see cref="IsLivePreviewMode"/> or <see cref="IsEditMode"/> is true. Also the opposite of <see cref="IsLiveMode"/>.
        /// </summary>
        /// <remarks>
        /// False if the Page Builder Mode cannot be determined (ex no HttpContext, Xperience features not initialized)
        /// </remarks>
        bool IsPreviewMode { get; }

        /// <summary>
        /// True if <see cref="IsLivePreviewMode"/> and <see cref="IsEditMode"/> is false. Also the opposite of <see cref="IsPreviewMode"/>
        /// </summary>
        /// <remarks>
        /// True if the Page Builder Mode cannot be determined (ex no HttpContext, Xperience features not initialized)
        /// </remarks>
        bool IsLiveMode { get; }

        /// <summary>
        /// True if the current request is being made for a preview version of the Page with editing disabled
        /// </summary>
        /// <remarks>
        /// False if the Page Builder Mode cannot be determined (ex no HttpContext, Xperience features not initialized)
        /// </remarks>
        bool IsLivePreviewMode { get; }

        /// <summary>
        /// True if the current request is being made for the Page Builder experience
        /// </summary>
        /// <remarks>
        /// False if the Page Builder Mode cannot be determined (ex no HttpContext, Xperience features not initialized)
        /// </remarks>
        bool IsEditMode { get; }

        /// <summary>
        /// The current Mode as a <see cref="PageBuilderMode" /> value
        /// </summary>
        PageBuilderMode Mode { get; }

        /// <summary>
        /// The value of <see cref="Mode" /> as a string
        /// </summary>
        string ModeName();
    }

    /// <summary>
    /// The various states that a request for a Page can be in, in relation to the Page Builder
    /// </summary>
    public enum PageBuilderMode
    {
        Live,
        LivePreview,
        Edit
    }
}
