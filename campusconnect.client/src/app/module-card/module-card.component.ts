import { Component, Input } from '@angular/core';
import { Module } from '../entities/module';
import { ModuleService } from '../services/module.service';

@Component({
  selector: 'app-module-card',
  templateUrl: './module-card.component.html',
  styleUrl: './module-card.component.scss'
})
export class ModuleCardComponent {

  @Input() module: Module = new Module;
  wasAdded: Boolean = false;

  constructor(public modService: ModuleService) {}

  public addModule(mod: Module) {
    this.modService.moduleList.push(mod);
    this.wasAdded = true;
  }

}
