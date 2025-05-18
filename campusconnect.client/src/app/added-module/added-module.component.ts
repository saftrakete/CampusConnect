import { Component, Input } from '@angular/core';
import { Module } from '../entities/module';
import { OnboardingService } from '../services/onboarding.service';

@Component({
  selector: 'app-added-module',
  templateUrl: './added-module.component.html',
  styleUrl: './added-module.component.scss'
})
export class AddedModuleComponent {
  @Input() module: Module = new Module;

  constructor(public os: OnboardingService) {}
}
