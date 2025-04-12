/// <reference types="Cypress"/>

describe("Kereső oldal tesztelése", () => {
    beforeEach(() => {
        cy.loginProfile('jancsika@gmail.com', 'jancsika');
        cy.get('[data-cy="searchboard_box"]').click();
        cy.scrollTo('top');
    }) 
    
    it('A táblázatban vannak felhasználók', () => {
        cy.get('[data-cy="users_table"] tbody tr').should('have.length', 5);
    })

    it('Keresésre megjelenik a felhasználó', () => {
        cy.get('[data-cy="search_username"]').type('evelin');
        cy.get('[data-cy="username_in_table"]').should('have.text', 'Evelin');
        cy.scrollTo('top');
    })

    it('Nem létezik ilyen felhasználó', () => {
        cy.get('[data-cy="search_username"]').type('asdas');
        cy.get('[data-cy="alert_messagebox"]').should('be.visible');
        cy.get('[data-cy="alert_messagebox"]').should('have.text', 'Nem létezik ez a felhasználó!');
        cy.get('[data-cy="alert_messagebox"]').should('have.css', 'background-color', 'rgb(255, 243, 205)');
        cy.scrollTo('top');
    })
})