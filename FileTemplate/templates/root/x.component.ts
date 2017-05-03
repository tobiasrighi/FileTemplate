import { Component, HostBinding } from '@angular/core';

/**
 * #name
 *
 * @export
 * @class #name
 */
@Component({
  moduleId: module.id,
  selector: 'lis-dr-#path',
  templateUrl: '#path.component.html'
})
export class #name {
  @HostBinding('class') hostClass: string = 'submodule-container';
}
