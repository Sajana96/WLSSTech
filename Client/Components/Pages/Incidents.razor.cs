using Client.AuthHandlers;
using Client.DTO;
using Client.Services;
using Common.Contracts;
using Common.Enums;
using Microsoft.AspNetCore.Components;
using System.ComponentModel.DataAnnotations;


namespace Client.Components.Pages
{
    public partial class Incidents
    {
        [Inject] private IncidentService Api { get; set; } = default!;
        [Inject] public TokenStore? TokenStore { get; set; } = default!;
        [Inject] private AuthLocalStorage AuthLocalStorage { get; set; }

        private readonly IncidentDto _create = new() { Severity = IncidentSeverity.Low};
        private List<IncidentResponse> _incidents = new();

        private bool _loading = true;
        private bool _busy;
        private string? _error;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (!firstRender) return;

            //var token = await AuthLocalStorage.GetTokenAsync();
            //if (token != null) 
            //    TokenStore.Set(token);

            await Load();

        }
        //protected override async Task OnafterRenderAsync()
        //{
        //}

        private async Task Load()
        {
            _loading = true;
            _error = null;

            try
            {
                var result = await Api.GetAllAsync();
                _incidents = result.Items;
            }
            catch (Exception ex)
            {
                _error = $"Failed to load incidents: {ex.Message}";
            }
            finally
            {
                _loading = false;
                StateHasChanged();
            }
        }

        private async Task CreateIncident()
        {
            _busy = true;
            _error = null;

            try
            {
                await Api.CreateAsync(new CreateIncidentRequest(_create.Title,_create.Description,_create.Severity,_create.Location));

                // reset form
                _create.Title = "";
                _create.Description = null;
                _create.Severity = IncidentSeverity.Low;
                _create.Location = null;

                await Load();
            }
            catch (Exception ex)
            {
                _error = $"Failed to create incident: {ex.Message}";
            }
            finally
            {
                _busy = false;
            }
        }

        private async Task ChangeStatus(Guid id, string? status)
        {
            if (string.IsNullOrWhiteSpace(status)) return;

            _busy = true;
            _error = null;

            try
            {
                await Api.UpdateStatusAsync(id, status);
                await Load();
            }
            catch (Exception ex)
            {
                _error = $"Failed to update status: {ex.Message}";
            }
            finally
            {
                _busy = false;
            }
        }

        private async Task Delete(Guid id)
        {
            _busy = true;
            _error = null;

            try
            {
                await Api.DeleteAsync(id);
                await Load();
            }
            catch (Exception ex)
            {
                _error = $"Failed to delete incident: {ex.Message}";
            }
            finally
            {
                _busy = false;
            }
        }
    }
}
