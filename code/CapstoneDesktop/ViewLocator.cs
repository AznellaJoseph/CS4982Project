using System;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using CapstoneDesktop.ViewModels;

namespace CapstoneDesktop
{
    /// <summary>
    ///     View Locator Class
    /// </summary>
    /// <seealso cref="Avalonia.Controls.Templates.IDataTemplate" />
    public class ViewLocator : IDataTemplate
    {
        /// <summary>
        ///     Builds the specified data.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        public IControl Build(object data)
        {
            var name = data.GetType().FullName!.Replace("ViewModel", "View");
            var type = Type.GetType(name);

            if (type != null)
                return (Control) Activator.CreateInstance(type)!;
            return new TextBlock {Text = "Not Found: " + name};
        }

        /// <summary>
        ///     Checks to see if this data template matches the specified data.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns>
        ///     True if the data template can build a control for the data, otherwise false.
        /// </returns>
        public bool Match(object data)
        {
            return data is ViewModelBase;
        }
    }
}