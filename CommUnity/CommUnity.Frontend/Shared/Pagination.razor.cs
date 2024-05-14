using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Options;

namespace CommUnity.FrontEnd.Shared
{
    public partial class Pagination
    {
        private List<PageModel> links = new();
        private List<RecordsNumberModel> recordsNumber = new();
        private int selectedOptionValue = 10;

        [Parameter] public int CurrentPage { get; set; } = 1;
        [Parameter] public int TotalPages { get; set; }
        [Parameter] public int Radio { get; set; } = 10;
        [Parameter] public EventCallback<int> SelectedPage { get; set; }
        [Parameter] public EventCallback<int> SelectedRecordsNumber { get; set; }
        [Parameter] public string? RecordsNumber { get; set; } = "10";

        private async Task InternalSelectedPage(PageModel pageModel)
        {
            if (pageModel.Page == CurrentPage || pageModel.Page == 0)
            {
                return;
            }

            await SelectedPage.InvokeAsync(pageModel.Page);
        }

        private async Task InternalRecordsNumberSelected(ChangeEventArgs e)
        {
            if (e.Value != null)
            {
                selectedOptionValue = Convert.ToInt32(e.Value.ToString());
            }
            await SelectedRecordsNumber.InvokeAsync(selectedOptionValue);
        }

        protected override void OnParametersSet()
        {
            BuildPages();
            BuildOptions();
        }

        private void BuildOptions()
        {
            recordsNumber =
            [
                new RecordsNumberModel { Value = 10, Name = "10" },
                new RecordsNumberModel { Value = 25, Name = "25" },
                new RecordsNumberModel { Value = 50, Name = "50" },
                new RecordsNumberModel { Value = int.MaxValue, Name = "Todos" },
            ];
        }


        private void BuildPages()
        {
            links = [];
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
            public string Name { get; set; } = null!;
            public int Value { get; set; }
        }
    }
}