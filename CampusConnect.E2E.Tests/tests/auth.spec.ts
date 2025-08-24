import { test, expect } from '@playwright/test';

test.describe('Authentication Flow', () => {
  test('should display login form', async ({ page }) => {
    await page.goto('/login');
    
    await expect(page.locator('input[formControlName="loginName"]')).toBeVisible();
    await expect(page.locator('input[formControlName="password"]')).toBeVisible();
    await expect(page.locator('button[type="submit"]')).toBeVisible();
  });

  test('should show validation errors for empty fields', async ({ page }) => {
    await page.goto('/login');
    
    await page.click('button[type="submit"]');
    
    await expect(page.locator('mat-error')).toBeVisible();
  });

  test('should navigate to register page', async ({ page }) => {
    await page.goto('/login');
    
    await page.click('text=Registrieren');
    
    await expect(page).toHaveURL('/register');
  });

  test('should display register form', async ({ page }) => {
    await page.goto('/register');
    
    await expect(page.locator('input[formControlName="loginName"]')).toBeVisible();
    await expect(page.locator('input[formControlName="nickname"]')).toBeVisible();
    await expect(page.locator('input[formControlName="password"]')).toBeVisible();
    await expect(page.locator('button[type="submit"]')).toBeVisible();
  });

  test('should show validation errors on register form', async ({ page }) => {
    await page.goto('/register');
    
    await page.click('button[type="submit"]');
    
    await expect(page.locator('mat-error')).toBeVisible();
  });
});