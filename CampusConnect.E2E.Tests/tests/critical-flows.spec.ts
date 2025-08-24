import { test, expect } from '@playwright/test';

test.describe('Critical User Flows', () => {
  test('complete registration flow', async ({ page }) => {
    await page.goto('/register');
    
    // Fill registration form
    await page.fill('input[formControlName="loginName"]', 'testuser');
    await page.fill('input[formControlName="nickname"]', 'Test User');
    await page.fill('input[formControlName="password"]', 'TestPassword123!');
    
    // Submit form
    await page.click('button[type="submit"]');
    
    // Should show success or redirect
    await expect(page.locator('.success-message, .mat-snack-bar-container')).toBeVisible({ timeout: 5000 });
  });

  test('login validation flow', async ({ page }) => {
    await page.goto('/login');
    
    // Try empty form
    await page.click('button[type="submit"]');
    await expect(page.locator('mat-error')).toBeVisible();
    
    // Fill invalid credentials
    await page.fill('input[formControlName="loginName"]', 'invalid');
    await page.fill('input[formControlName="password"]', 'wrong');
    await page.click('button[type="submit"]');
    
    // Should show error message
    await expect(page.locator('.error-message, .mat-snack-bar-container')).toBeVisible({ timeout: 5000 });
  });

  test('settings navigation flow', async ({ page }) => {
    await page.goto('/user-settings');
    
    // Navigate through settings sections
    const sections = ['Account Settings', 'Email Settings'];
    
    for (const section of sections) {
      if (await page.locator(`text=${section}`).isVisible()) {
        await page.click(`text=${section}`);
        await page.waitForTimeout(500); // Allow navigation
      }
    }
    
    await expect(page.locator('app-settings-sidebar')).toBeVisible();
  });
});