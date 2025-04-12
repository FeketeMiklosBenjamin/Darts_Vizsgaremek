Cypress.Commands.add('loginProfile', (email: string, password: string) => {
    cy.visit('/sign-in');
    cy.get('[data-cy="email_input"]').type(email);
    cy.get('[data-cy="password_input"]').type(password);
    cy.get('[data-cy="sign-in_btn"]').click();
});

/// <reference types="cypress" />
// ***********************************************
// This example commands.ts shows you how to
// create various custom commands and overwrite
// existing commands.
//
// For more comprehensive examples of custom
// commands please read more here:
// https://on.cypress.io/custom-commands
// ***********************************************
//
//
// -- This is a parent command --
//
//
// -- This is a child command --
// Cypress.Commands.add('drag', { prevSubject: 'element'}, (subject, options) => { ... })
//
//
// -- This is a dual command --
// Cypress.Commands.add('dismiss', { prevSubject: 'optional'}, (subject, options) => { ... })
//
//
// -- This will overwrite an existing command --
// Cypress.Commands.overwrite('visit', (originalFn, url, options) => { ... })
//
declare global {
  namespace Cypress {
    interface Chainable {
        loginProfile(email: string, password: string): Chainable<void>;
    }
  }
}


export {}
