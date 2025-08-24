import { test, expect } from '@playwright/test';

test.describe('User Settings', () => {
  test('should display account settings page', async ({ page }) => {
    await page.goto('/account-settings');
    
    await expect(page.locator('app-account-settings')).toBeVisible();
  });

  test('should show settings sidebar', async ({ page }) => {
    await page.goto('/user-settings');
    
    await expect(page.locator('app-settings-sidebar')).toBeVisible();
    await expect(page.locator('app-user-settings')).toBeVisible();
  });

  test('should navigate between settings sections', async ({ page }) => {
    await page.goto('/user-settings');
    
    await page.click('text=Account Settings');
    await expect(page).toHaveURL('/account-settings');
    
    await page.click('text=Email Settings');
    await expect(page).toHaveURL('/email-settings');
  });
});