import { test, expect } from '@playwright/test';

test.describe('Navigation', () => {
  test('should load home page', async ({ page }) => {
    await page.goto('/');
    
    await expect(page.locator('app-header')).toBeVisible();
    await expect(page.locator('app-home')).toBeVisible();
  });

  test('should navigate to login from header', async ({ page }) => {
    await page.goto('/');
    
    await page.click('text=Login');
    
    await expect(page).toHaveURL('/login');
  });

  test('should show 404 for invalid routes', async ({ page }) => {
    await page.goto('/invalid-route');
    
    await expect(page.locator('app-not-found')).toBeVisible();
  });
});