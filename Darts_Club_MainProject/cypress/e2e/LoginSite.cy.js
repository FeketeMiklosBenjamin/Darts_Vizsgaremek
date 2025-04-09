/// <reference types="Cypress"/>

describe("Bejelentkezés oldal tesztelése", () => {
    beforeEach(() => {
        cy.visit('/sign-in');
    })

    it('Bejelentkezés működik', () => {
        cy.get('[data-cy="email_input"]').type('jancsika@gmail.com');
        cy.get('[data-cy="password_input"]').type('jancsika');
        cy.get('[data-cy="sign-in_btn"]').click();
        cy.intercept('POST', '/api/login').as('postLogin');
        cy.get('[data-cy="loading_spinner"]').should('exist');
        cy.wait('@postLogin');
        cy.url().should('eq', 'http://localhost:5173/main-page');
    })

    it('Bejelentkezés sikertelen', () => {
        cy.get('[data-cy="email_input"]').type('aranyka@gmail.com');
        cy.get('[data-cy="password_input"]').type('aranyos');
        cy.get('[data-cy="sign-in_btn"]').click();
        cy.get('[data-cy="loading_spinner"]').should('exist');
    })
})