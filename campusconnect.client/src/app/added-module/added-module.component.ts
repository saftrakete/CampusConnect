import { Component, Input } from '@angular/core';
import { Module } from '../entities/module';
import { ModuleService } from '../services/module.service';

@Component({
  selector: 'app-added-module',
  templateUrl: './added-module.component.html',
  styleUrl: './added-module.component.scss'
})
export class AddedModuleComponent {
  @Input() module: Module = new Module;

  constructor(public modService: ModuleService) {}

public deleteModule(toBeDeletedModule: Module) {
  const index = this.modService.moduleList.findIndex(module => module.name === toBeDeletedModule.name);
  if (index !== -1) {
    this.modService.moduleList.splice(index, 1);
  }
}

}
