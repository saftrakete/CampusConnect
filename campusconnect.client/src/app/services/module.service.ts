import { Injectable } from '@angular/core';
import { Module } from '../entities/module';
import { ModuleCardComponent } from '../module-card/module-card.component';

@Injectable({
  providedIn: 'root'
})
export class ModuleService {
  moduleList: Module[] = [];
  constructor() { }

  // Checkt ob das übergebene Modul vom User geadded wurde
  public isCurrentlyAdded(mod: Module) {
    const index = this.moduleList.findIndex(m => m.name === mod.name);
    return !(index >= 0);
  }

}
