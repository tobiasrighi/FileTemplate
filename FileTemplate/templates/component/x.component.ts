import { Component, Input } from '@angular/core';

/**
 * #name
 *
 * @export
 * @class #name
 */
@Component({
  moduleId: module.id,
  selector: 'dr-#path',
  templateUrl: '#path.component.html',
})

export class #name {
  @Input() disabled: boolean = true;
  @Input() model: any;
}
