/// <reference types="Cypress"/>

describe("Bejelentkezés oldal tesztelése", () => {
    beforeEach(() => {
        cy.visit('/sign-in');
    })

    it('Bejelentkezés működik', () => {
        cy.intercept('POST', 'http://localhost:5181/api/users/login').as('login');
        cy.get('[data-cy="email_input"]').type('jancsika@gmail.com');
        cy.get('[data-cy="password_input"]').type('jancsika');
        cy.get('[data-cy="sign-in_btn"]').click();
        cy.get('[data-cy="loading_spinner"]').should('exist');
        cy.wait('@login');
        cy.url().should('eq', 'http://localhost:5173/main-page');
    })

    it('Bejelentkezés sikertelen', () => {
        cy.get('[data-cy="email_input"]').type('aranyka@gmail.com');
        cy.get('[data-cy="password_input"]').type('aranyos');
        cy.get('[data-cy="sign-in_btn"]').click();
        cy.get('[data-cy="error_message"]').should('have.text', 'Hibás email cím vagy jelszó.');
        cy.scrollTo('top');
    })
})