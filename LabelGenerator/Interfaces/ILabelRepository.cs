using System.Collections.Generic;
using LabelGenerator.Objects.LabelConfig;

namespace LabelGenerator.Interfaces
{
    /// <summary>
    /// Label repository
    /// </summary>
    public interface ILabelRepository
    {
        /// <summary>
        /// Fetch a label
        /// </summary>
        /// <param name="name">Name of the label</param>
        /// <returns>A Label</returns>
        LabelTemplate FetchLabel(string name);
        /// <summary>
        /// Fetches all labels
        /// </summary>
        /// <returns>A collection of labels</returns>
        IEnumerable<LabelTemplate> FetchAllLabels();

        /// <summary>
        /// Saves a label
        /// </summary>
        /// <param name="label">The label to be saved</param>
        /// <returns>A bool indicating success</returns>
        bool SaveLabel(LabelTemplate label);
        /// <summary>
        /// Saves all labels
        /// </summary>
        /// <param name="labels">A collection of labels to be saved</param>
        /// <returns>A bool</returns>
        bool SaveAllLabels(IEnumerable<LabelTemplate> labels);

        /// <summary>
        /// Deletes a label
        /// </summary>
        /// <param name="label">The label to be deleted</param>
        /// <returns>A bool</returns>
        bool DeleteLabel(LabelTemplate label);
    }
}
