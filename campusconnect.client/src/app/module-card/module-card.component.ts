import { Component, Input } from '@angular/core';
import { Module } from '../entities/module';

@Component({
  selector: 'app-module-card',
  templateUrl: './module-card.component.html',
  styleUrl: './module-card.component.scss'
})
export class ModuleCardComponent {
  @Input() module: Module = new Module;
}
