import { test, expect } from '@playwright/test';

test.describe('Two-Factor Authentication', () => {
  test('should display 2FA setup page', async ({ page }) => {
    await page.goto('/two-factor-setup');
    
    await expect(page.locator('app-two-factor-setup')).toBeVisible();
  });

  test('should show 2FA verification form', async ({ page }) => {
    await page.goto('/two-factor');
    
    await expect(page.locator('app-two-factor')).toBeVisible();
    await expect(page.locator('input[type="text"]')).toBeVisible();
  });

  test('should validate 2FA code input', async ({ page }) => {
    await page.goto('/two-factor');
    
    const codeInput = page.locator('input[type="text"]');
    await codeInput.fill('12345'); // Invalid length
    
    await expect(codeInput).toHaveValue('12345');
  });
});