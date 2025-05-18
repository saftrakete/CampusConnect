import { Component, Input } from '@angular/core';
import { Module } from '../entities/module';
import { OnboardingService } from '../services/onboarding.service';

@Component({
  selector: 'app-module-card',
  templateUrl: './module-card.component.html',
  styleUrl: './module-card.component.scss'
})
export class ModuleCardComponent {

  @Input() module: Module = new Module;
  isMenuOpen: Boolean = false;

  constructor(public os: OnboardingService) {}

  public toggleMenu() {
    this.isMenuOpen = !this.isMenuOpen;
  }
}
