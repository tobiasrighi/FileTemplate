import { #rootCompName } from './index';
import { #homeName, #routeName } from './home/index';
import { #formName } from './#clearName-form/index';

export const #name = [
  {
    path: '#clearName', component: #rootCompName, index: true,
    children: [
      ...#routeName,
      { path: 'home', component: #homeName },
      { path: 'show/:id', component: #formName },
      { path: 'show/:id/:tab', component: #formName },
      { path: 'edit/:id', component: #formName },
      { path: 'edit/:id/:tab', component: #formName },
      { path: 'create', component: #formName },
      { path: '', pathMatch: 'full', redirectTo: '/lis/dr/#clearName/home', terminal: true }
    ]
  }
];
