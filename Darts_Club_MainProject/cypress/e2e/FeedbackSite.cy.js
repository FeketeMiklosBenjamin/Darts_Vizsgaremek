/// <reference types="Cypress"/>

describe("Hibabejelentő oldal tesztelése", () => {
    beforeEach(() => {
        cy.loginProfile('jancsika@gmail.com', 'jancsika');
        cy.get('[data-cy="feedback_box"]').click();
        cy.scrollTo('top');
    })

    it('Sikertelen üzenetküldés', () => {
        cy.get('[data-cy="submit_button"]').click();
        cy.get('[data-cy="response_message"]').should('have.text', 'Az üzenet címe nem lehet üres.');
        cy.scrollTo('top');
    })

    it('Sikeres üzenetküldés', () => {
        cy.get('[data-cy="title_input"]').type('Ez egy üzenet');
        cy.get('[data-cy="subject_input"]').type("Itt látható egy üzenet");
        cy.get('[data-cy="submit_button"]').click();
        cy.get('[data-cy="response_message"]').should('have.text', 'Sikeresen elküldte az üzenetet!');
        cy.scrollTo('top');
    })
})