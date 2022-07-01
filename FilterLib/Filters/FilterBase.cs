using FilterLib.Reporting;
using System.Drawing;
using System.Text;
using System.Linq;

namespace FilterLib.Filters
{
    /// <summary>
    /// Base class for filters.
    /// </summary>
    public abstract class FilterBase : IFilter
    {
        /// <summary>
        /// Apply filter, the original image is not modified.
        /// </summary>
        /// <param name="image">Input image</param>
        /// <param name="reporter">Reporter (optional)</param>
        /// <returns>New image with filter applied</returns>
        public abstract Bitmap Apply(Bitmap image, IReporter reporter = null);

        /// <summary>
        /// Convert filter to string representation
        /// </summary>
        /// <returns>String representation of the filter</returns>
        public override string ToString()
        {
            StringBuilder sb = new();
            sb.Append(GetType().Name);
            var args = ReflectiveApi.GetFilterProperties(GetType())
                .Select(pi => $"{pi.Name}: {ParamToString(pi.GetValue(this))}");
            if (args.Any()) sb.Append($"({string.Join(", ", args)})");
            return sb.ToString();
        }

        /// <summary>
        /// Override this method to print parameters in custom format.
        /// </summary>
        /// <param name="param">Parameter to be printed</param>
        /// <returns>String representation</returns>
        public virtual string ParamToString(object param) => param.ToString();
    }
}
