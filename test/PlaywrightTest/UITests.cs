using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using NUnit.Framework;

namespace PlaywrightTest;

[Parallelizable(ParallelScope.Self)]
[TestFixture]
public class Tests : PageTest
{
    [Test]
    public async Task HomepageHasPlaywrightInTitleAndGetStartedLinkLinkingtoTheIntroPage()
    {
        await Page.GotoAsync("https://playwright.dev");

        // Expect a title "to contain" a substring.
        await Expect(Page).ToHaveTitleAsync(new Regex("Playwright"));

        // create a locator
        var getStarted = Page.GetByRole(AriaRole.Link, new() { Name = "Get started" });

        // Expect an attribute "to be strictly equal" to the value.
        await Expect(getStarted).ToHaveAttributeAsync("href", "/docs/intro");

        // Click the get started link.
        await getStarted.ClickAsync();

        // Expects the URL to contain intro.
        await Expect(Page).ToHaveURLAsync(new Regex(".*intro"));
    }
    
    /*
    [Test]
    public async Task UITest1()
    {
        await Page.GotoAsync('http://localhost:5000/');
        }/*
        await Page.GetByRole('link', { name: 'Register' }).ClickAsync();
        await Page.GetByPlaceholder('name@example.com').ClickAsync();

        //create random username
        let username = Math.random().toString(36).substring(7);

        await Page.GetByPlaceholder('name@example.com').FillAsync(username + '@test.dk');
        await Page.GetByPlaceholder('name@example.com').PressAsync('Tab');
        await Page.GetByLabel('Password', { exact: true }).FillAsync('Test123!');
        await Page.GetByLabel('Password', { exact: true }).PressAsync('Tab');
        await Page.GetByLabel('Confirm Password').FillAsync('Test123!');
        await Page.GetByRole('button', { name: 'Register' }).ClickAsync();
        await Page.GetByRole('link', { name: 'Click here to confirm your' }).ClickAsync();
        await Page.GetByRole('link', { name: 'Login' }).ClickAsync();
        await Page.GetByPlaceholder('name@example.com').ClickAsync();
        await Page.GetByPlaceholder('name@example.com').FillAsync(username + '@test.dk');
        await Page.GetByPlaceholder('name@example.com').PressAsync('Tab');
        await Page.GetByPlaceholder('password').FillAsync('Test123!');
        await Page.GetByRole('button', { name: 'Log in' }).ClickAsync();


        await Page.Locator('#CheepDTO_Text').ClickAsync();
        await Page.Locator('#CheepDTO_Text').FillAsync('This is my first cheep!');
        await Page.GetByRole('button', { name: 'Share' }).ClickAsync();

        //Testing if cheep is posted
        await Expect(Page.GetByText(username + '@test.dk This is my')).ToContainText('This is my first cheep!');

        await Page.GetByRole('link', { name: 'my timeline' }).ClickAsync();
        await Page.GetByRole('link', { name: 'public timeline' }).ClickAsync();
        await Page.Locator('li').Filter({ hasText: 'Quintin Sitts Follow It\'\'s' }).GetByRole('button').ClickAsync();

        //testing if button now writes unfollow
        await Expect(Page.Locator('li').Filter({ hasText: 'Quintin Sitts Unfollow It\'\'s' }).GetByRole('button')).ToContainText('Unfollow');

        await Page.GetByRole('link', { name: 'my timeline' }).ClickAsync();
        await Page.Locator('li').Filter({ hasText: 'Quintin Sitts Unfollow It\'\'s' }).GetByRole('button').ClickAsync();
        await Page.GetByRole('link', { name: 'public timeline' }).ClickAsync();
        await Page.Locator('li').Filter({ hasText: 'Jacqualine Gilcoine Follow Seems to me of Darmonodes\'\' elephant that so caused' }).GetByRole('link').ClickAsync();

        //Testing if the URL matches the author's usertimeline
        await await Expect(Page).toHaveURL('http://localhost:5000/Author/Jacqualine%20Gilcoine');

        await Page.Locator('li').Filter({ hasText: 'Jacqualine Gilcoine Follow Starbuck now is what we hear the worst. — 01-08-2023 13:17:' }).GetByRole('button').ClickAsync();
        await Page.GetByRole('link', { name: 'my timeline' }).ClickAsync();
        await Page.GetByRole('link', { name: 'public timeline' }).ClickAsync();
        await Page.Locator('li').Filter({ hasText: 'Quintin Sitts Follow It\'\'s' }).GetByRole('link').ClickAsync();
        await Page.GetByRole('link', { name: 'Logout' }).ClickAsync();
        await Page.GetByRole('button', { name: 'Click here to Logout' }).ClickAsync();
    }

    [Test]
    public async Task UITest2()
    {
        await Page.GotoAsync('http://localhost:5000/');

        await Expect(Page).ToHaveURL('http://localhost:5000/');

        await Page.GetByRole('link', { name: 'Register' }).ClickAsync();

        await Expect(Page).ToHaveURL('http://localhost:5000/Register');

        await Page.GetByPlaceholder('name@example.com').ClickAsync();

        //create random username
        let username = Math.random().toString(36).substring(7);

        await Page.GetByPlaceholder('name@example.com').FillAsync(username + '@test.dk');
        await Page.GetByLabel('Password', { exact: true }).ClickAsync();
        await Page.GetByLabel('Password', { exact: true }).FillAsync('Test123!');
        await Page.GetByLabel('Confirm Password').ClickAsync();
        await Page.GetByLabel('Confirm Password').FillAsync('Test123!');

        await Expect(Page.GetByLabel('Confirm Password')).toHaveValue('Test123!');

        await Page.GetByRole('button', { name: 'Register' }).ClickAsync();
        await Page.GetByRole('link', { name: 'Click here to confirm your' }).ClickAsync();
        await Page.GetByRole('link', { name: 'public timeline' }).ClickAsync();
        await Page.GetByRole('link', { name: 'Login' }).ClickAsync();
        await Page.GetByPlaceholder('name@example.com').ClickAsync();
        await Page.GetByPlaceholder('name@example.com').FillAsync(username + '@test.dk');
        await Page.GetByPlaceholder('name@example.com').PressAsync('Tab');
        await Page.GetByPlaceholder('password').FillAsync('Test123!');
        await Page.GetByRole('button', { name: 'Log in' }).ClickAsync();
    }*/
}