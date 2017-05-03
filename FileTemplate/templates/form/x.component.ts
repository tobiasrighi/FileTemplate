import { Component, HostBinding, OnInit, AfterViewInit } from '@angular/core';
import { FormBase } from '../../../../shared/forms/form-component-base/FormBase';

import { ModalDialogService } from '../../../../core/modal-dialog/index';
import { DataService } from '../../../../core/data/index';
import { StateService } from '../../../../core/state/index';
import { BusyIndicatorService } from '../../../../core/busy-indicator/index';

/**
 * #name
 *
 * @export
 * @class #name
 * @extends {FormBase}
 * @implements {OnInit}
 * @implements {AfterViewInit}
 */
@Component({
  moduleId: module.id,
  templateUrl: '#path.component.html'
})
export class #name extends FormBase implements OnInit, AfterViewInit {
  @HostBinding('class') hostClass: string = 'form-container';

  constructor(private modalDialogService: ModalDialogService) {
    super();
  }

  beforeSaveHandler() {
    let valid = this.validate();
    if (valid) {
      //n/a
    }
    return valid;
  }

  validate() {
    let IsValid = true;
    let errMess: Array<string> = [];

    if (!IsValid) {
        this.modalDialogService.info(errMess);
      }
    return IsValid;
  }

  afterModelLoadHandler() {
    //n/a
  }
}
