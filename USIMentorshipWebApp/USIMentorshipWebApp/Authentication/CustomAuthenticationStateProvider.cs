using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using System.Security.Claims;

namespace BlazorServerAuthenticationAndAuthorization.Authentication
{
    public class CustomAuthenticationStateProvider : AuthenticationStateProvider
    {
        // saving user details in this session storage
        private readonly ProtectedSessionStorage _sessionStorage;
        
        // using this object for anonomyous users (users who have no been authenticated)
        private ClaimsPrincipal _anonymous = new ClaimsPrincipal(new ClaimsIdentity());

        public CustomAuthenticationStateProvider(ProtectedSessionStorage sessionStorage)
        {
            _sessionStorage = sessionStorage;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            try
            {
                // reads the user session details from the session storage 
                // "UserSession" is the key of the session storage (key value format)
                // this means we are saving the logged in details with the key "UserSession" 
                var userSessionStorageResult = await _sessionStorage.GetAsync<UserSession>("UserSession");

                // if the retrieval was successful, userSession = userSessionStorageResult.Value
                // if it failed, userSession = null
                var userSession = userSessionStorageResult.Success ? userSessionStorageResult.Value : null;
                if (userSession == null)
                {
                    return await Task.FromResult(new AuthenticationState(_anonymous));
                }
                    
                else
                {
                    // creates claims principal object that represents the authenticated user
                    // ClaimsPrincipal contains multiple claims in a list (like passport)
                    // Claims are defined below (like information in passport)
                    // CustomAuth is the authentication type for the claims 
                    var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, userSession.UserName),
                        new Claim(ClaimTypes.Role, userSession.Role)
                    }, "CustomAuth"));
                    
                    // return authentication state with the newly created claimsPrincipal
                    return await Task.FromResult(new AuthenticationState(claimsPrincipal));
                }
                
            }
            catch
            {
                // return anonomyous authentication state
                return await Task.FromResult(new AuthenticationState(_anonymous));
            }
        }

        // update the authentication object and pass in the user session 
        public async Task UpdateAuthenticationState(UserSession userSession)
        {
            ClaimsPrincipal claimsPrincipal;

            if (userSession != null)
            {
                // assigning the value of userSession in session storage 
                await _sessionStorage.SetAsync("UserSession", userSession);
                
                // make a new claims principal with two claims of userName and role
                claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
                {
                    new Claim(ClaimTypes.Name, userSession.UserName),
                    new Claim(ClaimTypes.Role, userSession.Role)
                }));
            }

            // means user has logged out since userSession is null
            else
            {
                // delete the userSession value from the session storage 
                await _sessionStorage.DeleteAsync("UserSession");

                // change claims principal to anonoymous 
                claimsPrincipal = _anonymous;
            }

            // calling this method to pass authentication state and claims principal 
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(claimsPrincipal)));
        }
    }
}