import { Injectable } from '@angular/core';
import { Module } from '../entities/module';

@Injectable({
  providedIn: 'root'
})
export class ModuleService {
  moduleList: Module[] = [];
  constructor() { }
}
