using Blazored.Modal;
using Blazored.Modal.Services;
using CommUnity.FrontEnd.Pages.MyApartment;
using CommUnity.FrontEnd.Repositories;
using CommUnity.Shared.DTOs;
using CommUnity.Shared.Entities;
using CommUnity.Shared.Enums;
using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Net;

namespace CommUnity.FrontEnd.Pages.Worker
{
    public partial class Index
    {
        [Parameter] public int ResidentialUnitId { get; set; }
    }
}