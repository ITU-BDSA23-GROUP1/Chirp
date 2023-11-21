import { test, expect } from '@playwright/test';

test('testFollowAndUnfollow', async ({ page }) => {
  await page.goto('http://localhost:5000/');
  await page.getByRole('link', { name: 'Register' }).click();
  await page.getByPlaceholder('name@example.com').click();

  //create random username
  let username = Math.random().toString(36).substring(7);

  await page.getByPlaceholder('name@example.com').fill(username + '@test.dk');
  await page.getByPlaceholder('name@example.com').press('Tab');
  await page.getByLabel('Password', { exact: true }).fill('Test123!');
  await page.getByLabel('Password', { exact: true }).press('Tab');
  await page.getByLabel('Confirm Password').fill('Test123!');
  await page.getByRole('button', { name: 'Register' }).click();
  await page.getByRole('link', { name: 'Click here to confirm your' }).click();
  await page.getByRole('link', { name: 'Login' }).click();
  await page.getByPlaceholder('name@example.com').click();
  await page.getByPlaceholder('name@example.com').fill(username + '@test.dk');
  await page.getByPlaceholder('name@example.com').press('Tab');
  await page.getByPlaceholder('password').fill('Test123!');
  await page.getByRole('button', { name: 'Log in' }).click();


  await page.locator('#CheepDTO_Text').click();
  await page.locator('#CheepDTO_Text').fill('This is my first cheep!');
  await page.getByRole('button', { name: 'Share' }).click();

  //Testing if cheep is posted
  await expect(page.getByText(username+'@test.dk This is my')).toContainText('This is my first cheep!');

  await page.getByRole('link', { name: 'my timeline' }).click();
  await page.getByRole('link', { name: 'public timeline' }).click();
  await page.locator('li').filter({ hasText: 'Quintin Sitts Follow It\'\'s' }).getByRole('button').click();

  //testing if button now writes unfollow
  await expect(page.locator('li').filter({ hasText: 'Quintin Sitts Unfollow It\'\'s' }).getByRole('button')).toContainText('Unfollow');
  
  await page.getByRole('link', { name: 'my timeline' }).click();
  await page.locator('li').filter({ hasText: 'Quintin Sitts Unfollow It\'\'s' }).getByRole('button').click();
  await page.getByRole('link', { name: 'public timeline' }).click();
  await page.locator('li').filter({ hasText: 'Jacqualine Gilcoine Follow Seems to me of Darmonodes\'\' elephant that so caused' }).getByRole('link').click();

  //Testing if the URL matches the author's usertimeline
  await await expect(page).toHaveURL('http://localhost:5000/Author/Jacqualine%20Gilcoine');

  await page.locator('li').filter({ hasText: 'Jacqualine Gilcoine Follow Starbuck now is what we hear the worst. — 01-08-2023 13:17:' }).getByRole('button').click();
  await page.getByRole('link', { name: 'my timeline' }).click();
  await page.getByRole('link', { name: 'public timeline' }).click();
  await page.locator('li').filter({ hasText: 'Quintin Sitts Follow It\'\'s' }).getByRole('link').click();
  await page.getByRole('link', { name: 'Logout' }).click();
  await page.getByRole('button', { name: 'Click here to Logout' }).click();
});