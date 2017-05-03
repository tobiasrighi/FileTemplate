import { Component, HostBinding, OnInit } from '@angular/core';
import { HomeBase } from '../../../../shared/home-page/HomeBase';

import { DataService } from '../../../../core/data/index';
import { StateService } from '../../../../core/state/index';
import { BusyIndicatorService } from '../../../../core/busy-indicator/index';

/**
 * #name
 *
 * @export
 * @class #name
 */
@Component({
  moduleId: module.id,
  templateUrl: '#path.component.html'
})
export class #name extends HomeBase implements OnInit {

  @HostBinding('class') hostClass: string = 'home-page-container';

  entityType: string = 'DR.Folio';
  title: string;
  searchFields: any = { };

  constructor(public dataService: DataService, private stateService: StateService, public busyIndicatorService: BusyIndicatorService) {
    super();
  }

  ngOnInit() {
    let formOperations = this.stateService.currentState.formParams;
    this.title = formOperations.title;
  }
}
