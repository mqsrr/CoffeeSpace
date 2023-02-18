using System.Security.Claims;
using AutoMapper;
using CoffeeSpace.Data.Models.CustomerInfo;
using CoffeeSpace.WebAPI.Dto.Requests;
using CoffeeSpace.WebAPI.Dto.Response;
using CoffeeSpace.WebAPI.Services.Interfaces;
using CommunityToolkit.Diagnostics;
using Microsoft.AspNetCore.Identity;

namespace CoffeeSpace.WebAPI.Services;

public sealed class AccountService : IAccountService
{
    private readonly SignInManager<Customer> _signInManager;
    private readonly IMapper _mapper;
    private readonly ITokenProvider<Customer> _tokenProvider;

    public AccountService(SignInManager<Customer> signInManager, IMapper mapper, ITokenProvider<Customer> tokenProvider)
    {
        _signInManager = signInManager;
        _mapper = mapper;
        _tokenProvider = tokenProvider;
    }

    public async Task<JwtTokenResponse> LoginAsync(CustomerLoginModel customerViewModel, CancellationToken token = default!)
    {
        SignInResult signInResult = await _signInManager.PasswordSignInAsync(customerViewModel.UserName!, customerViewModel.Password, false, false);
        Guard.IsTrue(signInResult.Succeeded, nameof(signInResult));

        Customer? customer = await _signInManager.UserManager.FindByEmailAsync(customerViewModel.Email);
        Guard.IsNotNull(customer);
        
        JwtTokenResponse jwtToken = await _tokenProvider.GetTokenAsync(customer, token);

        return jwtToken;
    }

    public async Task<JwtTokenResponse> RegisterAsync(CustomerRegisterModel customerViewModel, CancellationToken token = default!)
    {
        Customer customer = _mapper.Map<Customer>(customerViewModel);

        IdentityResult identityResult = await _signInManager.UserManager.CreateAsync(customer, customer.Password);
        Guard.IsTrue(identityResult.Succeeded, nameof(identityResult));

        ClaimsPrincipal claimsPrincipal = await _signInManager.CreateUserPrincipalAsync(customer);

        await _signInManager.UserManager.AddClaimsAsync(customer, claimsPrincipal.Claims);
        
        SignInResult signInResult = await _signInManager.PasswordSignInAsync(customer,customer.Password, false, false);
        Guard.IsTrue(signInResult.Succeeded, nameof(signInResult));
        
        JwtTokenResponse jwtToken = await _tokenProvider.GetTokenAsync(customer, token);
        
        return jwtToken;
    }
}