import { test, expect } from '@playwright/test';

test('testRegistrationAndLogin', async ({ page }) => {

  await page.goto('http://localhost:5000/');

  await expect(page).toHaveURL('http://localhost:5000/');

  await page.getByRole('link', { name: 'Register' }).click();

  await expect(page).toHaveURL('http://localhost:5000/Register');

  await page.getByPlaceholder('name@example.com').click();

  //create random username
  let username = Math.random().toString(36).substring(7);

  await page.getByPlaceholder('name@example.com').fill(username + '@test.dk');
  await page.getByLabel('Password', { exact: true }).click();
  await page.getByLabel('Password', { exact: true }).fill('Test123!');
  await page.getByLabel('Confirm Password').click();
  await page.getByLabel('Confirm Password').fill('Test123!');

  await expect(page.getByLabel('Confirm Password')).toHaveValue('Test123!');

  await page.getByRole('button', { name: 'Register' }).click();
  await page.getByRole('link', { name: 'Click here to confirm your' }).click();
  await page.getByRole('link', { name: 'public timeline' }).click();
  await page.getByRole('link', { name: 'Login' }).click();
  await page.getByPlaceholder('name@example.com').click();
  await page.getByPlaceholder('name@example.com').fill(username + '@test.dk');
  await page.getByPlaceholder('name@example.com').press('Tab');
  await page.getByPlaceholder('password').fill('Test123!');
  await page.getByRole('button', { name: 'Log in' }).click();
});