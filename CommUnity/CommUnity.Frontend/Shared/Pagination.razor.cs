using Microsoft.AspNetCore.Components;

namespace CommUnity.FrontEnd.Shared
{
    public partial class Pagination
    {
        private List<PageModel> links = new();
        private List<RecordsNumberModel> recordsNumber = new();

        [Parameter] public int CurrentPage { get; set; } = 1;
        [Parameter] public int TotalPages { get; set; }
        [Parameter] public int Radio { get; set; } = 10;
        [Parameter] public EventCallback<int> SelectedPage { get; set; }
        [Parameter] public EventCallback<string> SelectedRecordsNumber { get; set; }
        [Parameter] public string? RecordsNumber { get; set; } = "10";

        private readonly List<string> values = new()
        {
            "10",
            "25",
            "50",
            "todos"
        };

        private async Task InternalSelectedPage(PageModel pageModel)
        {
            if (pageModel.Page == CurrentPage || pageModel.Page == 0)
            {
                return;
            }

            await SelectedPage.InvokeAsync(pageModel.Page);
        }

        private async Task ChangeRecordsPerPage(ChangeEventArgs e)
        {
            RecordsNumber = e.Value?.ToString();
            await SelectedRecordsNumber.InvokeAsync(RecordsNumber);
        }

        protected override void OnInitialized()
        {
            for (int i = 1; i <= values.Count; i++)
            {
                recordsNumber.Add(new RecordsNumberModel
                {
                    Text = values[i - 1],
                    Value = values[i - 1]
                });
            }
        }

        protected override void OnParametersSet()
        {
            links = new List<PageModel>();
            var previousLinkEnable = CurrentPage != 1;
            var previousLinkPage = CurrentPage - 1;

            links.Add(new PageModel
            {
                Text = "Anterior",
                Page = previousLinkPage,
                Enable = previousLinkEnable
            });

            for (int i = 1; i <= TotalPages; i++)
            {
                if (TotalPages <= Radio)
                {
                    links.Add(new PageModel
                    {
                        Page = i,
                        Enable = CurrentPage == i,
                        Text = $"{i}"
                    });
                }

                if (TotalPages > Radio && i <= Radio && CurrentPage <= Radio)
                {
                    links.Add(new PageModel
                    {
                        Page = i,
                        Enable = CurrentPage == i,
                        Text = $"{i}"
                    });
                }

                if (CurrentPage > Radio && i > CurrentPage - Radio && i <= CurrentPage)
                {
                    links.Add(new PageModel
                    {
                        Page = i,
                        Enable = CurrentPage == i,
                        Text = $"{i}"
                    });
                }
            }

            var linkNextEnable = CurrentPage != TotalPages;
            var linkNextPage = CurrentPage != TotalPages ? CurrentPage + 1 : CurrentPage;
            links.Add(new PageModel
            {
                Text = "Siguiente",
                Page = linkNextPage,
                Enable = linkNextEnable
            });
        }

        private class PageModel
        {
            public string Text { get; set; } = null!;
            public int Page { get; set; }
            public bool Enable { get; set; } = true;
            public bool Active { get; set; } = false;
        }

        private class RecordsNumberModel
        {
            public string Text { get; set; } = null!;
            public string Value { get; set; } = null!;
        }
    }
}