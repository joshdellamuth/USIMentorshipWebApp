using Microsoft.Extensions.Caching.Memory;
using USIMentorshipWebApp.Data;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.Diagnostics.Metrics;
using USIMentorshipWebApp.Models;

using System;


public class PasswordService
{
    private readonly IMemoryCache _memoryCache;
    private readonly EmailService _emailService;

    public PasswordService(IMemoryCache memoryCache, EmailService emailService)
    {
        _memoryCache = memoryCache;
        _emailService = emailService;
    }

    //This function is used to configure the password reset feature
    public async Task SendPasswordResetCode(string email)
    {
        var random = new Random();
        var code = random.Next(1000, 9999).ToString(); // Generate a four-digit code
        var cacheEntryOptions = new MemoryCacheEntryOptions()
            .SetAbsoluteExpiration(TimeSpan.FromMinutes(15)); // Code expires in 15 minutes

        // Store code in memory cache
        _memoryCache.Set(email, code, cacheEntryOptions);

        // Send the code to the user's email asynchronously
        await _emailService.SendMail(email, "Password Reset Code", $"Your password reset code is {code}");
    }
    public Task<bool> VerifyResetCodeAsync(string email, string inputCode)
    {
        if (_memoryCache.TryGetValue(email, out string? storedCode))
        {
            return Task.FromResult(storedCode == inputCode);
        }
        return Task.FromResult(false);
    }

    public async Task<bool> UpdateUserPassword(string email, string newPassword)
    {
        using UsiMentorshipApplicationContext userContext = new UsiMentorshipApplicationContext();

        var user = await userContext.Users.FirstOrDefaultAsync(u => u.EmailAddress == email);
        if (user == null) return false;

        user.Password = newPassword;  // Use a strong hashing algorithm
        userContext.Users.Update(user);
        await userContext.SaveChangesAsync();
        return true;
    }
}

