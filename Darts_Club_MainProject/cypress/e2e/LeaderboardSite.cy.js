/// <reference types="Cypress"/>

describe("Ranglista oldal tesztelése", () => {
    beforeEach(() => {
        cy.loginProfile('jancsika@gmail.com', 'jancsika');
        cy.get('[data-cy="leaderboard_box"]').click();
        cy.scrollTo('top');
    }) 
    
    it('Szint kiválasztó sávban megjelennek a játékosok', () => {
        cy.get('[data-cy="selection_level"]').select('Professional');
        cy.scrollTo('top');
        cy.get('[data-cy="users_username"]').should('have.text', 'Hajnalodik');
        cy.get('[data-cy="index_of_user"]').should('have.text', '1.');
        cy.get('[data-cy="index_of_user"]').should('have.css', 'color', 'rgb(255, 215, 13)');
    })

    it('Nincs adott szinten játékos', () => {
        cy.get('[data-cy="selection_level"]').select('Advanced');
        cy.get('[data-cy="alert_messagebox"]').should('be.visible');
        cy.get('[data-cy="alert_messagebox"]').should('have.text', 'Nincsen ilyen szintű felhasználó!');
        cy.get('[data-cy="alert_messagebox"]').should('have.css', 'background-color', 'rgb(255, 243, 205)');
        cy.scrollTo('top');
    })
})