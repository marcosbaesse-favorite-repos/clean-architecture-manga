﻿namespace Manga.UI.Presenters
{
    using Manga.Application;
    using Manga.Application.UseCases.CloseAccount;
    using Microsoft.AspNetCore.Mvc;

    public class ClosePresenter : IOutputBoundary<CloseOutput>
    {
        public IActionResult ViewModel { get; private set; }

        public CloseOutput Output { get; private set; }

        public void Populate(CloseOutput response)
        {
            Output = response;

            if (response == null)
            {
                ViewModel = new NoContentResult();
                return;
            }

            ViewModel = new OkResult();
        }
    }
}