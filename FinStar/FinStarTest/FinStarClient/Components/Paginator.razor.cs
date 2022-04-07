using Microsoft.AspNetCore.Components;

namespace FinStarClient.Components
{
    public partial class Paginator
    {
        [Parameter] 
        public int PageCount { get; set; }

        [Parameter] 
        public int CurrentPage { get; set; }

        [Parameter] 
        public EventCallback<int> CurrentPageChanged { get; set; }

        [Parameter] 
        public bool ShowFirstLast { get; set; } = false;

        [Parameter] 
        public bool ShowPageNumbers { get; set; } = true;

        [Parameter] 
        public string FirstText { get; set; } = String.Empty;

        [Parameter] 
        public string LastText { get; set; } = String.Empty;

        [Parameter] 
        public string PreviousText { get; set; } = String.Empty;

        [Parameter] 
        public string NextText { get; set; } = String.Empty;

        [Parameter] 
        public int VisiblePages { get; set; } = 5;

        private void PagerButtonClicked(int page)
        {
            CurrentPage = page;
            CurrentPageChanged.InvokeAsync(CurrentPage);
        }
    }
}
