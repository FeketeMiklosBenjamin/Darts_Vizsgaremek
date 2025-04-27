/// <reference types="Cypress"/>

describe("Versenyek oldal tesztelése", () => {
    beforeEach(() => {
        cy.loginProfile('jancsika@gmail.com', 'jancsika');
        cy.get('[data-cy="competition_box"]').click();
        cy.scrollTo('top');
    })

    it('Van egy verseny', () => {
        cy.get('[data-cy="competition_card"]').should('have.length', 1)
    })

    it('Jelentkezés a megfelelő versenyre', () => {
        cy.get('[data-cy="competition_name"]').should('have.text', 'Teszt verseny')
        cy.get('[data-cy="check-in-btn"]').click()
        cy.get('[data-cy="applicate_button"]').click()
        cy.scrollTo('top');
    })

    it('Nincsen új verseny', () => {
        cy.get('[data-cy="alert_message"]').should('have.text', 'Nincs meghírdetett verseny az ön szintjén!')
        cy.scrollTo('top');
    })

    it('Regisztrált verseny megtekintése', () => {
        cy.get('[data-cy="switch"]').click()
        cy.get('[data-cy="text_in_switch"]').should('not.have.text', 'Nevezés')
        cy.get('[data-cy="text_in_switch"]').should('have.text', 'Regisztrált')
        cy.get('[data-cy="competition_card"]').should('have.length', 1)
        cy.scrollTo('top');
    })
})