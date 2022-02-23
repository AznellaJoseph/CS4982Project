using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReactiveUI;

namespace CapstoneDesktop.ViewModels
{
    /// <summary>
    /// ReactiveViewModelBase Class that serves as a base for ViewModels that need reactive functionality 
    /// </summary>
    /// <seealso cref="CapstoneDesktop.ViewModels.ViewModelBase" />
    /// <seealso cref="ReactiveUI.IRoutableViewModel" />
    public class ReactiveViewModelBase : ViewModelBase, IRoutableViewModel
    {
        /// <summary>
        ///     The host screen.
        /// </summary>
        public IScreen HostScreen { get; set; }

        /// <summary>
        ///     The url path segment.
        /// </summary>
        public string? UrlPathSegment { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ReactiveViewModelBase"/> class.
        /// </summary>
        /// <param name="hostScreen">The host screen.</param>
        /// <param name="urlPathSegment">The URL path segment.</param>
        public ReactiveViewModelBase(IScreen hostScreen, string? urlPathSegment)
        {
            this.HostScreen = hostScreen;
            this.UrlPathSegment = urlPathSegment;
        }
    }
}
