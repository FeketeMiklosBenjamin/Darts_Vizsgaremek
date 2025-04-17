<template>
    <div class="vt-item">
        <div v-if="bracketNode.match" :class="getBracketNodeClass(bracketNode)">
            <div v-if="bracketNode.match.feedIn" class="vt-item-feedIn">
                <div class="title">{{ bracketNode.match.title }}</div>
                <div :class="['vt-feedIn', getHighlightClass(bracketNode.match.feedIn.id)]"
                    @mouseover="highlightTeam(bracketNode.match.feedIn.id)" @mouseleave="unhighlightTeam"
                    @click="onMatchClick(bracketNode.match.id)">
                    <span class="name">{{ bracketNode.match.feedIn.name }}</span>
                </div>
            </div>

            <div v-else-if="bracketNode.match.team1 && bracketNode.match.team2" class="vt-item-teams">
                <div class="title">{{ bracketNode.match.title }}</div>

                <div :class="['vt-team', 'vt-team-top', getTeamClass(bracketNode.match.team1)]"
                    @mouseover="highlightTeam(bracketNode.match.team1.id)" @mouseleave="unhighlightTeam"
                    @click="onMatchClick(bracketNode.match.id)">
                    <span class="name">{{ bracketNode.match.team1.name }}</span>
                    <span class="score">{{ bracketNode.match.team1.score }}</span>
                </div>

                <div :class="['vt-team', 'vt-team-bot', getTeamClass(bracketNode.match.team2)]"
                    @mouseover="highlightTeam(bracketNode.match.team2.id)" @mouseleave="unhighlightTeam"
                    @click="onMatchClick(bracketNode.match.id)">
                    <span class="name">{{ bracketNode.match.team2.name }}</span>
                    <span class="score">{{ bracketNode.match.team2.score }}</span>
                </div>
            </div>
        </div>

        <div v-if="bracketNode.children?.length" class="vt-item-children">
            <div v-for="(child, i) in bracketNode.children" :key="i" class="vt-item-child">
                <Bracket :bracket-node="child" :highlighted-team-id="highlightedTeamId" @onMatchClick="onMatchClick"
                    @onSelectedTeam="highlightTeam" @onDeselectedTeam="unhighlightTeam" />
            </div>
        </div>
    </div>
</template>


<script setup lang="ts">
import { defineProps, defineEmits } from 'vue';
import type IBracketNode from '@/interface/IBracketNode';
import type ITeam from '@/interface/ITeam';

const props = defineProps<{
    bracketNode: IBracketNode;
    highlightedTeamId?: string;
}>();

const emit = defineEmits<{
    (e: 'onMatchClick', matchId: string): void
    (e: 'onSelectedTeam', teamId: string): void
    (e: 'onDeselectedTeam'): void
}>();

const onMatchClick = (matchId: string) => {
    emit('onMatchClick', matchId);
};

const highlightTeam = (teamId: string) => {
    emit('onSelectedTeam', teamId);
};

const unhighlightTeam = () => {
    emit('onDeselectedTeam');
};

const getBracketNodeClass = (node: IBracketNode) => {
    if (node.children?.length) return 'vt-item-parent';
    if (node.hasParent) return 'vt-item-child';
    return '';
};

const getTeamClass = (team: ITeam): string => {
    let clazz = '';
    
    if (props.bracketNode.match?.winner === team.id) {
        clazz = 'winner';
    }
    if (props.highlightedTeamId === team.id) {
        clazz += ' highlight';
    }
    return clazz;
};

const getHighlightClass = (teamId: string): string => {
    return props.highlightedTeamId === teamId ? 'highlight' : '';
};

</script>
<style scoped>

.vt-item {
    display: flex;
    flex-direction: row-reverse;
}

.vt-item-parent {
    position: relative;
    margin-left: 50px;
    display: flex;
    align-items: center;
}

.vt-item-parent:after {
    position: absolute;
    content: "";
    width: 25px;
    height: 2px;
    left: 0;
    top: 50%;
    background-color: black;
    transform: translateX(-100%) translateY(-50%);
}

.vt-item-children {
    display: flex;
    flex-direction: column;
    justify-content: center;
}

.vt-item-child {
    display: flex;
    align-items: flex-start;
    justify-content: flex-end;
    margin: 10px 0;
    position: relative;
}

.vt-item-child:before {
    content: "";
    position: absolute;
    background-color: black;
    right: 0;
    top: 50%;
    transform: translateX(100%) translateY(-50%);
    width: 25px;
    height: 2px;
}

.vt-item-child:after {
    content: "";
    position: absolute;
    background-color: black;
    right: -25px;
    height: calc(50% + 10px);
    width: 2px;
    top: 50%;
}

.vt-item-child:last-child:after {
    transform: translateY(-100%);
}

.vt-item-child:only-child:after {
    display: none;
}

/* Meccs dizájn */
.vt-item-teams,
.vt-item-feedIn {
    flex-direction: column;
    margin-bottom: 16px;
}

.vt-item-teams .title,
.vt-item-feedIn .title {
    font-size: 10px;
    margin-bottom: 1px;
}

.vt-team,
.vt-feedIn {
    display: flex;
    width: 140px;
    font-size: 10px;
    height: 25px;
    cursor: pointer;
    background-color: white;
    border: 1px solid black; /* Alapértelmezett fekete border */
    box-sizing: border-box; /* A border beleszámít a fix szélességbe/magasságba */
}



.vt-team .name,
.vt-feedIn .name {
    padding: 3px 6px;
    width: 110px;
    white-space: nowrap;
    overflow: hidden;
    text-overflow: ellipsis;
}

.vt-team .score {
    width: 20px;
    padding: 3px 6px;
    text-align: center;
}

.vt-team-top {
    border-top: 2px solid #414146;
    border-radius: 5px 5px 0 0;
}

.vt-team-bot {
    border-radius: 0 0 5px 5px;
}

.winner {
    background-color: rgb(212, 168, 57);
}

.highlight {
    border: 2px solid red;
}
</style>