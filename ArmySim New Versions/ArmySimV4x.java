/**
 * @(#)ArmySimV4x.java
 *
 *GOAL: UPDATE ARMY SIM WITH ITEMS AND SPELLS AND DIFFERENT TERRAIN AND BALANCE CHANGES ***REDESIGN UNIT CLASS TO BE ONE CLASS***
 *FUTURE PLANS: ADD WEAPON CLASS AND GIVE WEAPONS TO EACH SOLDIER
 *EXPERIMENT GOAL: ADD AN INVENTORY UI
 *
 * @author Nathan Frazier
 * @version 4.85x 2015/12/18
 *
 * MAIN BALANCE CHANGES TO NOTE : 
 *Cavalry can now only attack 2 spaces away, not 1.
 *Archer can only attack 4-8 spaces away, not within the first 3 tiles.
 *Soldiers can build defence which lasts one turn - DELAYED
 *Wizard can attack from 2-3 tiles away
 *
 *CHECKLIST
 *[X] Redesign unit class to be one class - 100%
 *[X] Add the wizard class - 100%    ERROR - NULL POINTER WHEN USING FIRE STRIKE ON MORE THAN ONE UNIT
 *[X] Add different terrain - 50%   ADDED FIRE - DELAYED STONE
 *[X] Make balance changes - 100%    BEEN LAZY ON THIS ONE BECAUSE IT WILL TAKE A LOT OF WORK AND RECODING A LOT
 *[X] Add an Inventory UI - 100%
 */
 
package solution;
import static java.lang.String.*;
import java.awt.*;
import java.awt.event.*;
import javax.swing.*;

import static java.lang.System.*;
import static javax.swing.JOptionPane.*;
import javax.swing.JFrame;
import javax.swing.JPanel;
import javax.swing.ImageIcon;
import java.awt.Color;
import java.awt.Graphics2D;
import java.awt.Graphics;
import java.awt.BasicStroke;
import java.awt.Font;
import java.util.ArrayList;
import java.util.Arrays;
import java.util.*;
import java.io.*;
import static javax.swing.JOptionPane.*;
import javax.swing.JOptionPane;
import java.awt.event.MouseEvent;
import java.awt.event.MouseAdapter;


public class ArmySimV4x {
    public static void main(String[] args) {
    	new Environment();
    }
}

class Environment extends View
{
	protected int pad = 2;
	ArrayList<ImageIcon> icons = new ArrayList<ImageIcon>();

	int[][] board = { 	//	  1  2  3  4  5  6  7  8  9
			/*1*/			{ 0, 0, 0, 0, 0, 0, 0, 0, 0},
			/*2*/			{ 0, 0, 0, 0, 0, 0, 0, 0, 0},
			/*3*/			{ 0, 0, 0, 0, 0, 0, 0, 0, 0},
			/*4*/			{ 0, 0, 0, 0, 0, 0, 0, 0, 0},
			/*5*/			{ 0, 0, 0, 0, 0, 0, 0, 0, 0},
			/*6*/			{ 0, 0, 0, 0, 0, 0, 0, 0, 0},
			/*7*/			{ 0, 0, 0, 0, 0, 0, 0, 0, 0},
			/*8*/			{ 0, 0, 0, 0, 0, 0, 0, 0, 0},
			/*9*/			{ 0, 0, 0, 0, 0, 0, 0, 0, 0},
			/*10*/			{ 0, 0, 0, 0, 0, 0, 0, 0, 0},
			/*11*/			{ 0, 0, 0, 0, 0, 0, 0, 0, 0},
			/*12*/			{ 0, 0, 0, 0, 0, 0, 0, 0, 0},
			/*13*/			{ 0, 0, 0, 0, 0, 0, 0, 0, 0},
			/*14*/			{ 0, 0, 0, 0, 0, 0, 0, 0, 0},
			/*15*/			{ 0, 0, 0, 0, 0, 0, 0, 0, 0}
								};
	
	HashMap<String,Unit> friendlyUnits;
	HashMap<String,Unit> enemyUnits;
	int numArchers,numKnights,numCavalry;
	
	ArrayList<String> names;
	Scanner scan;
	private int cash=100000;
	private int tutorial;
	String temp;
	
	private boolean enemyMoved;
	private boolean enemyAttacked;
	
	HashMap<String,Point> connections;
	ArrayList<String> turn;
	private int actions;
	
	boolean win, lose;
	
	ArrayList<Weapon> allWeapons;
	ArrayList<Unit> invUnits;
	
	double chanceOfFindingWeapon = .3; /////////CHANGE THIS BACK TO .3 WHEN DONE TESTING CHANGE TO 1.0 TO ALWAYS GET A DROP
	
	public Environment()
	{
		allWeapons = new ArrayList<Weapon>();
		invUnits = new ArrayList<Unit>();
		
		icons.add(new ImageIcon( "grass2.jpg" ));
		icons.add(new ImageIcon( "knight2.png" )); // 1
		icons.add(new ImageIcon( "knight2x.png" ));// 2
		icons.add(new ImageIcon( "archer2.png" ));// 3
		icons.add(new ImageIcon( "archer2x.png" ));// 4
		icons.add(new ImageIcon( "cav2.png" ));// 5
		icons.add(new ImageIcon( "cav2x.png" )); // 6
		icons.add(new ImageIcon( "knight1.png" ));// 7
		icons.add(new ImageIcon( "knight1x.png" ));// 8
		icons.add(new ImageIcon( "archer1.png" ));// 9
		icons.add(new ImageIcon( "archer1x.png" ));// 10
		icons.add(new ImageIcon( "cav1.png" ));// 11
		icons.add(new ImageIcon( "cav1x.png" )); // 12
		icons.add(new ImageIcon( "savage.png" )); // 13
		icons.add(new ImageIcon( "savagex.png" )); // 14
		icons.add(new ImageIcon( "wizard.png" )); // 15
		icons.add(new ImageIcon( "wizardx.png" )); // 16
		friendlyUnits = new HashMap<String,Unit>();
		names = new ArrayList<String>();
		try{
			scan = new Scanner(new File("allNames.dat"));
			while(scan.hasNext()){
				names.add(scan.nextLine().trim());
			}
			
		}catch(Exception e){System.out.println("didn't read in file!\n"+e);}
	}
	
	@Override
	protected void drawPieces(Graphics2D g)
	{
		for(int r=0; r<numRows; r++)
		  for(int c=0; c<numCols; c++)
			if ( board[r][c]>0 && board[r][c]<icons.size() )
			  g.drawImage(icons.get(board[r][c]).getImage(),c*os+pad+5,r*os+pad+5,null);
		repaint();
	}

		@Override
	public void buttonOneAction()  //////// BATTLE &&& SPELL ONE
	{
		if(inBattle){
			if(buttonOne.getText().equals("EMPTY"))
				JOptionPane.showMessageDialog(null,"No Item or Spell is in that slot!","No Item/Spell",JOptionPane.WARNING_MESSAGE);
			else{
				if(spellTwoOn)
					hideSpellRange();
				spellTwoOn=false;
				if(spellOneOn)
					spellOneOn=false;
				else
					spellOneOn=true;
					
				switch(friendlyUnits.get(turn.get(0)).getSpellOne()){
					case "Fire Strike":
						spellRange =3;
						break;
					case "Fire Spread":
						spellRange =4;
						break;
				}
				if(spellOneOn)
					showSpellRange();
				else
					hideSpellRange();
			}
		}
		else{
			if (friendlyUnits.size()==0){
				JOptionPane.showMessageDialog(null,"You need units before you can fight anyone!","No Units",JOptionPane.WARNING_MESSAGE);
			}
			else{
				if(tutorial==0){
					inBattle=true;
					setBattleParams();
					int[][] matrix;
					numKnights=numCavalry=numArchers=0;
					for(Map.Entry<String, Unit> element : friendlyUnits.entrySet()){
						switch(element.getValue().getType()){
							case "k": numKnights++; break;
							case "c": numCavalry++; break;
							case "a": numArchers++; break;
							case "w": numKnights++; numArchers++; break;
						}
					}
					matrix = troopSpawn(0,numKnights,numCavalry,numArchers);
					output.setText("\n  Objective  :  Defeat your Enemies\n\n");
					repaint();
					chooseLocation(board);
					getActions();
					updateUnderMap();
					showCurrentTurn();	
					repaint();
				}
				else
					if(tutorial!=4){
						output.append("  Finish the tutorial before starting a Battle!\n\n");
						tutorialBattle();
					}
					else
						tutorialBattle();
			}
		}
	}

	@Override
	public void buttonTwoAction()  //////// TRAIN &&& SPELL TWO
	{
		if(inBattle){
			if(buttonOne.getText().equals("EMPTY"))
				JOptionPane.showMessageDialog(null,"No Item or Spell is in that slot!","No Item/Spell",JOptionPane.WARNING_MESSAGE);
			else{
				if(spellOneOn)
					hideSpellRange();
				spellOneOn=false;
				if(spellTwoOn)
					spellTwoOn=false;
				else
					spellTwoOn=true;
					
				switch(friendlyUnits.get(turn.get(0)).getSpellTwo()){
					case "Fire Strike":
						spellRange =3;
						break;
					case "Fire Spread":
						spellRange =4;
						break;
				}
				if(spellTwoOn)
					showSpellRange();
				else
					hideSpellRange();
			}
		}
		else{
			if (friendlyUnits.size()==0){
				JOptionPane.showMessageDialog(null,"You need units before you can fight anyone!","No Units",JOptionPane.WARNING_MESSAGE);
			}
			else{
				if(tutorial==0||tutorial==3){
					int[][] matrix;
					inBattle=true;
					setBattleParams();
					int savNum = (int)(Math.random()*6)+5;
					matrix = troopSpawn(savNum,0,0,0);
					output.setText("\n  Defeat your Enemies\n\n");
					repaint();
					chooseLocation(board);
					getActions();
					updateUnderMap();
					showCurrentTurn();
					repaint();
				}
				else
					if(tutorial<2)
						JOptionPane.showMessageDialog(null,"Hire some men before trying to train!","Complete Tutorial",JOptionPane.WARNING_MESSAGE);
					else {
						tutorialTrain();
					}
			}
		}
	}
	
	@Override
	public void buttonThreeAction()   //////// VIEW ARMY &&& END TURN
	{
		if(inBattle){
			endTurn();
			showCurrentTurn();
			if(lose){
				resetAllSpells();
				resetBoard();
				resetUnderMap();
				resetTerrainMap();
				setParams();
				lose = false;
				inBattle=false;
				int losses = 200;
				cash-=losses;
				output.setText("\n  You lost the battle and your men are wounded.\n  You must pay "+losses+" gold to be released.\n\n");
				
				if (cash<0){
					data.setText("0");
					JOptionPane.showMessageDialog(null,"You have run out of money.. The king takes away your position as commander!","Game Over",JOptionPane.PLAIN_MESSAGE);
					System.exit(0);
				}
				data.setText(""+cash);
				for(Map.Entry<String, Unit> element : friendlyUnits.entrySet()){
					element.getValue().heal();
				}
			}
		}
		else{
			if (tutorial==3){
				output.append("  Here you can see all the men in your army.\n  You can also change their weapons on this screen.\n  It is important to note that any unit that is killed in battle is permanently dead.\n");
				output.append("  If you win your men will automatically be healed up and ready to go.\n  If you retreat your men will NOT be healed, and will be more likely to die in your next battle.\n"+
					"  Now you are ready for a REAL battle.\n  Hit the Battle button after you look over your army to start the final part of the tutorial.\n\n");
				tutorial++;
			}
			invUnits.clear();
			weapons.clear();
			for(Map.Entry<String, Unit> element : friendlyUnits.entrySet()){
				weapons.add(element.getValue().getWeaponImageName());
				invUnits.add(element.getValue());
			}
			allWeapons.add(new Weapon("Sword","Rare"));
			setInventoryParams();
			fightUI=false;
			invUI=true;
			buttonThree.setVisible(false);
			buttonFour.setVisible(false);
			data.setVisible(false);
			dataLabel.setVisible(false);
			scroll.setVisible(false);
			test.setRows(100);
			repaint();
		}
	}

	@Override
	public void buttonFourAction()   //////// BUY UNIT &&& RETREAT
	{
		if(inBattle){
			setParams();
			resetUnderMap();
			resetTerrainMap();
			inBattle=false;
			turn.clear();
			resetBoard();
			connections.clear();
			output.setText("\n  You have retreated from battle!\n  Coward..\n\n");
			if (tutorial==2)
				tutorial++;
			resetAllSpells();
		}
		else{
			if(friendlyUnits.size()>=16){
				JOptionPane.showMessageDialog(null,"Your army is at max capacity.","Army Size: "+friendlyUnits.size(),JOptionPane.WARNING_MESSAGE);
			}
			else{
				String[] temp={"Knight - 200g","Cavalry - 1000g","Archer - 400g","Wizard - 600g"};
				int choice = JOptionPane.showOptionDialog(null, 
	     		 "Which Soldier would you like to buy?", 
	     		 "Hire Soldier", 
	     		 0, 
	     		 JOptionPane.QUESTION_MESSAGE, 
	     		 null, temp, null);
	     		 
	     		switch(choice){
	     			case 0: if(cash>=200){
	     				cash-=200;
	     				data.setText(""+cash);
	     				String name = findName();
	     				friendlyUnits.put(name,new Unit(name,"k"));
	     				if(tutorial==1){
	     					output.setText("  Good job!\n  You have bought your first Soldier.\n  You can view the stats of the new Soldier you have hired below.\n\n");
	     					output.append("  ****New Knight Registration Form:****\n"+friendlyUnits.get(name));
	     					output.append("  Buy more units to increase the size of your army.\n  After that try going out and fighting savages for Gold, Fame, and Experience!\n  You can do this by clicking on the Train button at the bottom.\n\n");
	     					tutorial++;
	     				}
	     				else
	     					output.append("  ****New Knight Registration Form:****\n"+friendlyUnits.get(name));
	     			}
	     			else{
	     				JOptionPane.showMessageDialog(null,"You don't have enough Gold!","Your Gold: "+cash,JOptionPane.WARNING_MESSAGE);
	     				buttonFourAction();
	     			} 
	     			break;
	     			case 1: if(cash>=1000){
	     				cash-=1000;
	     				data.setText(""+cash);
	     				String name = findName();
	     				friendlyUnits.put(name,new Unit(name,"c"));
	     				if(tutorial==1){
	     					output.setText("  Good job!\n  You have bought your first Soldier.\n  You can view the stats of the new Soldier you have hired below.\n\n");
	     					output.append("  ****New Cavalry Registration Form:****\n"+friendlyUnits.get(name));
	     					output.append("  Buy more units to increase the size of your army.\n  After that try going out and fighting savages for Gold, Fame, and Experience!\n  You can do this by clicking on the Train button at the bottom.\n\n");
	     					tutorial++;
	     				}
	     				else
	     					output.append("  ****New Cavalry Registration Form:****\n"+friendlyUnits.get(name));
	     				
	     			}
	     			else{
	     				JOptionPane.showMessageDialog(null,"You don't have enough Gold!","Your Gold: "+cash,JOptionPane.WARNING_MESSAGE);
	     				buttonFourAction();
	     			} 
		     			break;
		     		case 2: if(cash>=400){
		     			cash-=400;
		     			data.setText(""+cash);
		     			String name = findName();
		     			friendlyUnits.put(name,new Unit(name,"a"));
		     			if(tutorial==1){
		     				output.setText("  Good job!\n  You have bought your first Soldier.\n  You can view the stats of the new Soldier you have hired below.\n\n");
		     				output.append("   ****New Archer Registration Form:****\n"+friendlyUnits.get(name));
		     				output.append("  Buy more units to increase the size of your army.\n  After that try going out and fighting savages for Gold, Fame, and Experience!\n  You can do this by clicking on the Train button at the bottom.\n\n");
		     				tutorial++;
		     			}
		     			else
		     				output.append("   ****New Archer Registration Form:****\n"+friendlyUnits.get(name));
		     			
		     		}
		     		else{
		     			JOptionPane.showMessageDialog(null,"You don't have enough Gold!","Your Gold: "+cash,JOptionPane.WARNING_MESSAGE);
		     			buttonFourAction();
		     		} 
		     			break;
		     			
		     		case 3: if(cash>=600){
		     			cash-=600;
		     			data.setText(""+cash);
		     			String name = findName();
		     			friendlyUnits.put(name,new Unit(name,"w"));
		     			if(tutorial==1){
		     				output.setText("  Good job!\n  You have bought your first Soldier.\n  You can view the stats of the new Soldier you have hired below.\n\n");
		     				output.append("   ****New Wizard Registration Form:****\n"+friendlyUnits.get(name));
		     				output.append("  Buy more units to increase the size of your army.\n  After that try going out and fighting savages for Gold, Fame, and Experience!\n  You can do this by clicking on the Train button at the bottom.\n\n");
		     				tutorial++;
		     			}
		     			else
		     				output.append("   ****New Wizard Registration Form:****\n"+friendlyUnits.get(name));
		     			
		     		}
		     		else{
		     			JOptionPane.showMessageDialog(null,"You don't have enough Gold!","Your Gold: "+cash,JOptionPane.WARNING_MESSAGE);
		     			buttonFourAction();
		     		} 
		     			break;
	     		}	
			}
		}
	}
	////////////////////////////////////////CONVERSIONS BELOW
	private int findRow(int c){ 
		int row = 12;
     	while(c>=9){
     	 	c-=9;
     	 	row++;
     	 }
     	 return row;
	}
	private int findCol(int c){  
		int col = 0;
		while(c>=9){
     	 	c-=9;
     	 }
     	 while(c>0){
     	 	c--;
     	 	col++;
     	 }
		return col;
	}
	////////////////////////////////////////CHOOSING LOCATION BELOW **VERY LONG AND COMPLICATED**
	private int[][] chooseLocation(int[][] m){
		int[][] matrix = m;
		badAlive=0;
		goodAlive=0;
		String[] temp={ "[01]", "[02]", "[03]", "[04]", "[05]", "[06]", "[07]", "[08]", "[09]",
						 "[11]", "[12]", "[13]", "[14]", "[15]", "[16]", "[17]", "[18]", "[19]",
						 "[21]", "[22]", "[23]", "[24]", "[25]", "[26]", "[27]", "[28]", "[29]"};
		int choice = 0;
		for(Map.Entry<String, Unit> element : friendlyUnits.entrySet()){
			String question = "";
			if(element.getValue().getType().equals("k"))
				question = "Knight";
			if(element.getValue().getType().equals("c"))
				question = "Cavalry";
			if(element.getValue().getType().equals("a"))
				question = "Archer";
			if(element.getValue().getType().equals("w"))
				question = "Wizard";
					
			choice = JOptionPane.showOptionDialog(null, 
     		 "Where would you like to place your "+question+"?", 
     		 "Choose Location", 
     		 0, 
     		 JOptionPane.QUESTION_MESSAGE, 
     		 null, temp, null);
     		while(matrix[findRow(choice)][findCol(choice)]!=0)
				choice = JOptionPane.showOptionDialog(null, 
	     		 "You already have a unit there, place your "+question+" somewhere else!", 
	     		 "Choose Location", 
	     		 0, 
	     		 JOptionPane.QUESTION_MESSAGE, 
	     		 null, temp, null);
	     	if(element.getValue().getType().equals("k"))
				matrix[findRow(choice)][findCol(choice)] = 7;
			if(element.getValue().getType().equals("c"))
				matrix[findRow(choice)][findCol(choice)] = 11;
			if(element.getValue().getType().equals("a"))
				matrix[findRow(choice)][findCol(choice)] = 9;
			if(element.getValue().getType().equals("w"))
				matrix[findRow(choice)][findCol(choice)] = 15;
     		connections.put(element.getKey(),new Point(findRow(choice),findCol(choice)));
     		turn.add(element.getKey());
     		
     		if(tutorial==3){
     			output.setText("\n  In a Battle you will face a lot more than just Savages!\n  The enemy will have the same troops you do, so be careful!\n");
	   			output.append("  Your Opponent will set up his troops, and you then will choose where to set up yours.\n  Do that now...\n\n");		
     		}
     		if(tutorial==2){
     			output.setText("\n  In Training you will fight against tribes of Savages, which are very weak units, but in numbers they can cause some damage.\n");
				output.append("  Start any fight by moving your units into range, then all you need to do is attack them.\n  Set up your units where you want to quickly wipe out the Savages!\n\n");
     		}
     		if(tutorial==0){
     			output.setText("\n  Defeat your Enemies\n\n");
     		}
     		
	   		
		    repaint();
		}
		
		for (int row=12; row<matrix.length; row++)
	    {
			for (int col=0; col<matrix[0].length; col++)
				if(matrix[row][col]!=7&&matrix[row][col]!=9&&matrix[row][col]!=11&&matrix[row][col]!=15)
					matrix[row][col]=0;
	    }
	    Collections.shuffle(turn);
	//  System.out.println(turn);
	    
	    while(!turn.get(0).contains(" ")){
	   			Collections.rotate(turn,-1);
	   	}
	   	for(int c=0;c<turn.size();c++){
	   			if(turn.get(c).contains(" "))
	   				goodAlive++;
	   			else
	   				badAlive++;
	   	}
	   	board = matrix;
	   	setUnitParams(turn.get(0));
		return matrix;
		
	}
	
	//////////////////////////////////////// BATTLE STUFF
	
	private ArrayList<String> inRange(){
		ArrayList<String> allEnemies = new ArrayList<String>();
		String name = turn.get(0);
		Point l = connections.get(name);
		int range = 0;
		boolean enemy = false;
		if(friendlyUnits.containsKey(name))
			range = friendlyUnits.get(name).getRange();
		else{
			range = enemyUnits.get(name).getRange();
			enemy = true;
		}
					
		for(Map.Entry<String, Point> element : connections.entrySet()){
			String temp = element.getKey();
			for(int r = 1;r<=range;r++){
				if (!enemy){
					if(!temp.contains(" ")){
						if(element.getValue().x==l.x){
							if(element.getValue().y==l.y-r)
								allEnemies.add(element.getKey());
							if((element.getValue().y==l.y+r))
								allEnemies.add(element.getKey());
						}
						if(element.getValue().y==l.y){
							if(element.getValue().x==l.x-r)
								allEnemies.add(element.getKey());
							if((element.getValue().x==l.x+r))
								allEnemies.add(element.getKey());
						}	
					}
				}
				else {
					if(temp.contains(" ")){
						if(element.getValue().x==l.x){
							if(element.getValue().y==l.y-r)
								allEnemies.add(element.getKey());
							if((element.getValue().y==l.y+r))
								allEnemies.add(element.getKey());
						}
						if(element.getValue().y==l.y){
							if(element.getValue().x==l.x-r)
								allEnemies.add(element.getKey());
							if((element.getValue().x==l.x+r))
								allEnemies.add(element.getKey());
						}	
					}
				}
			}
			
			
		}
		return allEnemies;
	}
	public void chooseAttack2(){
		String realName = "";
		ArrayList<String> selection = inRange();
		if(selection!=null&&selection.size()>0){
			for(int x=0;x<selection.size();x++){
				if(connections.get(selection.get(x)).equals(new Point(realX,realY)))
					realName=selection.get(x);
			}
			attack(realName);
		}
		
	}
	public boolean checkAttack(){
		String realName = "";
		ArrayList<String> selection = inRange();
		if(selection!=null&&selection.size()>0){
			for(int x=0;x<selection.size();x++){
				if(connections.get(selection.get(x)).equals(new Point(realX,realY)))
					return true;
			}
		}
		return false;
	}
	public boolean unitThere(){
		if((board[realX][realY]>0&&board[realX][realY]<7)||((board[realX][realY]>12)&&(board[realX][realY]<15)))
			return true;
		return false;
	}
		
	private void attack(String n){
		int damage = 0;
		String name = turn.get(0);
		damage = friendlyUnits.get(name).getDPS();
		
		showCurrentTurn();
		try{
			if(enemyUnits.get(n).damage(damage)){
				output.append(""+n+" took "+ damage+" damage!\n");
			}
			else{
				board[connections.get(n).x][connections.get(n).y] = 0;
				connections.remove(n);
				turn.remove(n);
				showCurrentTurn();
				enemyUnits.remove(n);
				badAlive--;
				output.append(""+n+" died..\n");
				addWeapon();
				checkEnd();
			}
		}catch(Exception e){}
		if(inBattle&&!spellOneOn&&!spellTwoOn)			
			endTurn();		
			
	}
	///////////////////////////////////////////////////SPELLS
	public void spellOneEffect(int x,int y){
		if(underMap[x][y]>3){
			if(friendlyUnits.get(turn.get(0)).useSpellOne()){
				switch(friendlyUnits.get(turn.get(0)).getSpellOne()){
					case "Fire Strike":
						fireStrike(x,y);
						break;
					case "Fire Spread":
						fireSpread(x,y);
						break;
				}
				usedSpell=true;
			}
			else
				JOptionPane.showMessageDialog(null,"Can't use that Item/Spell anymore!","No Item/Spell Uses",JOptionPane.WARNING_MESSAGE);
			spellOneOn=false;
			hideSpellRange();	
			setUnitParams(turn.get(0));
		}
		else
			JOptionPane.showMessageDialog(null,"That spot is not in your Item/Spell range!","Out of Item/Spell Range",JOptionPane.WARNING_MESSAGE);		
		if(usedSpell){
			usedSpell=false;
			checkEnd();
			endTurn();
		}
	}
	public void spellTwoEffect(int x,int y){
		if(underMap[x][y]>3){
			if(friendlyUnits.get(turn.get(0)).useSpellTwo()){
				switch(friendlyUnits.get(turn.get(0)).getSpellTwo()){
					case "Fire Strike":
						fireStrike(x,y);
						break;
					case "Fire Spread":
						fireSpread(x,y);
						break;
				}
				usedSpell=true;	
			}
			else
				JOptionPane.showMessageDialog(null,"Can't use that Item/Spell anymore!","No Item/Spell Uses",JOptionPane.WARNING_MESSAGE);
			spellTwoOn=false;
			hideSpellRange();
			setUnitParams(turn.get(0));
			}
		else
			JOptionPane.showMessageDialog(null,"That spot is not in your Item/Spell range!","Out of Item/Spell Range",JOptionPane.WARNING_MESSAGE);	
		if(usedSpell){
			usedSpell=false;
			checkEnd();
			endTurn();
		}
	}
	public void showSpellRange(){
		Point location = connections.get(turn.get(0));
		for(int r=location.x+(spellRange*(-1));r<=location.x+spellRange;r++){
			for(int c=location.y+(spellRange*(-1));c<=location.y+spellRange;c++){
				if(r>=0&&r<15&&c>=0&&c<9){
					underMap[r][c]+=4;
				}
			}	
		}
		repaint();
	}
	public void hideSpellRange(){
		Point location = connections.get(turn.get(0));
		for(int r=location.x+(spellRange*(-1));r<=location.x+spellRange;r++){
			for(int c=location.y+(spellRange*(-1));c<=location.y+spellRange;c++){
				if(r>=0&&r<15&&c>=0&&c<9){
					underMap[r][c]-=4;
				}
			}	
		}
		repaint();
	}
	
	public void fireStrike(int xPos,int yPos){
		String name = "";
		terrain[xPos][yPos]=1;
		if((board[xPos][yPos]>0&&board[xPos][yPos]<7)||((board[xPos][yPos]>12)&&(board[xPos][yPos]<15))){
			name =spellHit(xPos,yPos);
			if(!name.equalsIgnoreCase("NOTHING")){
				attack(name);
			}
				
		}
		if(xPos-1>=0){
			terrain[xPos-1][yPos]=1;
			if((board[xPos-1][yPos]>0&&board[xPos-1][yPos]<7)||((board[xPos-1][yPos]>12)&&(board[xPos-1][yPos]<15))){
				name =spellHit(xPos-1,yPos);
				if(!name.equalsIgnoreCase("NOTHING")){
					attack(name);
				}
					
			}
		}
		if(xPos+1<15){
			terrain[xPos+1][yPos]=1;
			if((board[xPos+1][yPos]>0&&board[xPos+1][yPos]<7)||((board[xPos+1][yPos]>12)&&(board[xPos+1][yPos]<15))){
				name =spellHit(xPos+1,yPos);
				if(!name.equalsIgnoreCase("NOTHING")){
					attack(name);
				}
					
			}
		}	
		
		
		if(yPos-1>=0){
			terrain[xPos][yPos-1]=1;
			if((board[xPos][yPos-1]>0&&board[xPos][yPos-1]<7)||((board[xPos][yPos-1]>12)&&(board[xPos][yPos-1]<15))){
				name =spellHit(xPos,yPos-1);
				if(!name.equalsIgnoreCase("NOTHING")){
					attack(name);
				}
					
			}
		}	
		if(yPos+1<9){
			terrain[xPos][yPos+1]=1;
			if((board[xPos][yPos+1]>0&&board[xPos][yPos+1]<7)||((board[xPos][yPos+1]>12)&&(board[xPos][yPos+1]<15))){
				name =spellHit(xPos,yPos+1);
				if(!name.equalsIgnoreCase("NOTHING")){
					attack(name);
				}
					
			}
		}	
		
		
		
		repaint();
	}
	public void fireSpread(int xPos,int yPos){
		if(xPos-1>=0){
			if(yPos-1>=0)
				terrain[xPos-1][yPos-1]=1;
			if(yPos+1<9)
				terrain[xPos-1][yPos+1]=1;
		}
		if(xPos+1<15){
			if(yPos-1>=0)
				terrain[xPos+1][yPos-1]=1;
			if(yPos+1<9)
				terrain[xPos+1][yPos+1]=1;
		}
		terrain[xPos][yPos]=1;
		repaint();
			
	}
	public String spellHit(int RealX,int RealY){
		String realName = "";
		ArrayList<String> selection= new ArrayList<String>();
		Point spot = new Point(RealX,RealY);
		for(Map.Entry<String, Unit> element : enemyUnits.entrySet()){
			selection.add(element.getKey());
		}
		if(selection!=null&&selection.size()>0){
			for(int x=0;x<selection.size();x++){
				if(connections.get(selection.get(x)).equals(spot)){ 
					realName=selection.get(x);
				}
					
			}
			return realName;
		}
	    return "NOTHING";
	}
	///////////////////////////////////////////////////CHECK END. VERY IMPORTANT
	private void checkEnd(){
		win = true;
		lose = true;
	//	System.out.println(turn);
		for(Map.Entry<String, Point> element : connections.entrySet()){
			if(element.getKey().contains(" ")){
				lose = false;
			}
			else{
				win = false;
			}
		}
		if(lose){
				resetAllSpells();
				resetBoard();
				setParams();
				lose = false;
				resetUnderMap();
				resetTerrainMap();	
				inBattle=false;
				int losses = 200;
				cash-=losses;
				repaint();
				output.setText("\n  You lost the battle and your men are wounded.\n  You must pay "+losses+" gold to be released.\n\n");
				
				if (cash<0){
					data.setText("0");
					JOptionPane.showMessageDialog(null,"You have run out of money.. The king takes away your position as commander!","Game Over",JOptionPane.PLAIN_MESSAGE);
					System.exit(0);
				}
				data.setText(""+cash);
			}
			if(win){
				resetAllSpells();
				healUnits();
				resetBoard();
				win = false;
				setParams();
				resetUnderMap();
				resetTerrainMap();
				inBattle=false;
				int gains = 300;
				cash+=gains;
				for(Map.Entry<String, Unit> element : friendlyUnits.entrySet()){
					element.getValue().heal();
				}
				output.setText("");
				if (tutorial==2||tutorial ==4){
					if(tutorial ==2){
						output.append("  Congrats!\n  You have won your first battle.\n  The king is proud of you, adding to your fame, and he rewards you will some gold!\n"+
							"  You can also keep any gold found on the Savages.\n  Below it will show you your gains.\n  See what you gained then view your army with the View Army button below.\n\n");
						tutorial++;
					}
						
					if(tutorial ==4){
						output.append("  Congrats!\n  You have won your first REAL battle.\n  The king is VERY proud of you, GREATLY adding to your fame, and he rewards you will A LOT of gold!\n"+
							"  You can also keep any gold found on the Enemy army.\n  This Concludes the tutorial.\n  Good Luck Commander!\n\n");
						tutorial=0;
					}
						
					
				}
				repaint();
				output.append("\n  You won the battle by killing all of your enemies!\n  You gained "+gains+" gold.\n\n");
				data.setText(""+cash);
					
			}
	}
	
	////////////////////////////////////////MOVE
	@Override
	public void moveUnit2(){
		try{
			
			if(turn.get(0).contains(" ")){
				if(board[realX][realY]==0){
					String[] list = {"Yes","No"};
					int choice = JOptionPane.showOptionDialog(null, 
			     		 "Do you really want to move there?", 
			     		 "Move", 
			     		 0, 
			     		 JOptionPane.QUESTION_MESSAGE, 
			     		 null, list, null);
					if(choice ==0){
						
						String name = turn.get(0);	
						Point location = new Point();
						location = connections.get(name);
						board[location.x][location.y] = 0;
						int distance = Math.abs(realX-location.x)+Math.abs(realY-location.y);
						actions-=distance;
						location = new Point(realX,realY);
						connections.put(name,location);
						updateUnderMap();
				//		System.out.println(realX+"   "+realY);
						repaint();
						showCurrentTurn();	
					}
				}
				else{
					JOptionPane.showMessageDialog(null,"There is already a unit there!","Invalid Move",JOptionPane.ERROR_MESSAGE);
				}
			}
			else
				System.out.println("Why?");
			
		}catch(Exception e){System.out.println("BROKEN");}
	}
	//////////////////////////////////////// AI
	private void moveAI(){  
		String name = turn.get(0);
		Point location = new Point();
		location = connections.get(name);
		ArrayList<Point> allL = new ArrayList<Point>();
		for(Map.Entry<String, Point> element : connections.entrySet()){
			allL.add(element.getValue());
		}
		
		getActions();
		boolean isTurn = true;
		Point goal = goalAI(location);
		while(actions>0&&isTurn){
			boolean clearLeft = true;
			boolean clearRight = true;
			boolean clearForward = true;
			boolean clearBackward = true;
			for(Point element:allL){
				if(element.x==location.x){
					if(element.y==location.y-1)
						clearLeft = false;
					if((element.y==location.y+1))
						clearRight = false;
				}
				if(element.y==location.y){
					if(element.x==location.x-1)
						clearForward = false;
					if((element.x==location.x+1))
						clearBackward = false;
				}
					
			}
			if(attackAI()){
				isTurn=false;
				enemyAttacked=true;
			}
			else{
				board[location.x][location.y] = 0;
				if(Math.random()<=.75){
					if(goal.x>location.x&&clearBackward)
						location.x+=1;
					else
						if(goal.x<location.x&&clearForward)
							location.x-=1;
						else
							if(goal.y<location.y&&clearLeft)
								location.y-=1;
							else
								if(goal.y>location.y&&clearRight)
									location.y+=1;
								else
									isTurn=false;		
				}
				else{
					if(goal.y>location.y&&clearRight)
						location.y+=1;
					else
						if(goal.y<location.y&&clearLeft)
							location.y-=1;
						else
							if(goal.x<location.x&&clearForward)
								location.x-=1;
							else
								if(goal.x>location.x&&clearBackward)
									location.x+=1;
								else
									isTurn=false;
				}
					
				actions--;
				connections.put(name,location);
				showCurrentTurn();
				enemyMoved=true;
			}
		}
		if(!enemyAttacked){
			if(attackAI()){
				enemyAttacked=true;
			}
		}
		if(lose){
			inBattle=false;
			
		}
		else{
			if(inBattle)
				endTurn();
		//	showCurrentTurn();
		}
		
	}
	private Point goalAI(Point l){
		ArrayList<Point> allL = new ArrayList<Point>();
		int distance = 30;
		int temp = 0;
		Point goal = new Point();
		for(Map.Entry<String, Point> element : connections.entrySet()){
			if(element.getKey().contains(" ")){
				temp=Math.abs(l.x-element.getValue().x)+Math.abs(l.y-element.getValue().y);
				if (temp<distance){
					distance = temp;
					goal = element.getValue();
				}
				
			}
		}
		return goal;
	}
	private boolean attackAI(){
		String name = turn.get(0);
		int damage = 0;
		ArrayList<String> allEnemies = inRange();
		if(allEnemies.size()<1)
			return false;
		else{
			String n = allEnemies.get((int)Math.random()*allEnemies.size());
			
			damage = enemyUnits.get(name).getDPS();
			
			if(friendlyUnits.get(n).damage(damage)){
				output.append(""+n+" took "+ damage+" damage!\n");
			}
			else{
				board[connections.get(n).x][connections.get(n).y] = 0;
				connections.remove(n);
				turn.remove(n);
				friendlyUnits.remove(n);
				goodAlive--;
			//	showCurrentTurn();
				output.append(""+n+" died..\n");
				checkEnd();
			}
		}
		
		return true;
	}
	////////////////////////////////////////TURN DATA BELOW
	private void endTurn(){
		String type = "";
		String name = turn.get(0);	
		if(friendlyUnits.containsKey(name))
			type = friendlyUnits.get(name).getType();
		else
			type = enemyUnits.get(name).getType();
		if(terrain[connections.get(name).x][connections.get(name).y]==1){
			if(friendlyUnits.containsKey(name)){
				if(friendlyUnits.get(name).damage(50))
					output.append(name+" took 50 damage from standing in fire!\n");
				else{
					board[connections.get(name).x][connections.get(name).y] = 0;
					connections.remove(name);
					turn.remove(name);
					friendlyUnits.remove(name);
					goodAlive--;
					output.append(name+" died from standing in fire..\n");
					checkEnd();
					
				}
			}
			else{
				if(enemyUnits.get(name).damage(50))
					output.append(enemyUnits.get(name).getRealType()+" took 50 damage from standing in fire!\n");
				else{
					board[connections.get(name).x][connections.get(name).y] = 0;
					connections.remove(name);
					turn.remove(name);
					badAlive--;
					output.append(enemyUnits.get(name).getRealType()+" died from standing in fire..\n");
					checkEnd();
					
				}	
			}	
		}
		if(inBattle){
			switch(type){
				case "k": board[connections.get(turn.get(0)).x][connections.get(turn.get(0)).y] = 7; break;
				case "c": board[connections.get(turn.get(0)).x][connections.get(turn.get(0)).y] = 11; break;
				case "a": board[connections.get(turn.get(0)).x][connections.get(turn.get(0)).y] = 9; break;
				case "ek": board[connections.get(turn.get(0)).x][connections.get(turn.get(0)).y] = 1; break;
				case "ec": board[connections.get(turn.get(0)).x][connections.get(turn.get(0)).y] = 5; break;
				case "ea": board[connections.get(turn.get(0)).x][connections.get(turn.get(0)).y] = 3; break;
				case "s": board[connections.get(turn.get(0)).x][connections.get(turn.get(0)).y] = 13; break;
				case "w": board[connections.get(turn.get(0)).x][connections.get(turn.get(0)).y] = 15; break;
			};
			Collections.rotate(turn,-1);
			getActions();
			updateUnderMap();
			showCurrentTurn();
			if(friendlyUnits.containsKey(turn.get(0))){
				setUnitParams(turn.get(0));
			}
				
	
			if(!turn.get(0).contains(" ")&&inBattle){
				moveAI();	
			}
			enemyMoved=false;
			enemyAttacked=false;	
		}
		
		
	}
	
	private void showCurrentTurn(){
		
		String type = "";
		if(friendlyUnits.containsKey(turn.get(0)))
			type = friendlyUnits.get(turn.get(0)).getType();
		else
			type = enemyUnits.get(turn.get(0)).getType();
		switch(type){
			case "k": board[connections.get(turn.get(0)).x][connections.get(turn.get(0)).y] = 8; break;
			case "c": board[connections.get(turn.get(0)).x][connections.get(turn.get(0)).y] = 12; break;
			case "a": board[connections.get(turn.get(0)).x][connections.get(turn.get(0)).y] = 10; break;
			case "ek": board[connections.get(turn.get(0)).x][connections.get(turn.get(0)).y] = 2; break;
			case "ec": board[connections.get(turn.get(0)).x][connections.get(turn.get(0)).y] = 6; break;
			case "ea": board[connections.get(turn.get(0)).x][connections.get(turn.get(0)).y] = 4; break;
			case "s": board[connections.get(turn.get(0)).x][connections.get(turn.get(0)).y] = 14; break;
			case "w": board[connections.get(turn.get(0)).x][connections.get(turn.get(0)).y] = 16; break;
		};
		repaint();
	}
	////////////////////////////////////////TUTORIAL BATTLES BELOW
	private void tutorialBattle(){
		inBattle=true;
		setBattleParams();
		int[][] matrix = troopSpawn(0,2,0,1);
		tutorial = 0;
		
		
	    output.setText("\n  In a Battle you will face a lot more than just Savages!\n  The enemy will have the same troops you do, so be careful!\n");
	   	output.append("  Your Opponent will set up his troops, and you then will choose where to set up yours.\n  Do that now...\n\n");		
	    repaint();
	    matrix=chooseLocation(matrix);
	    getActions();
	    updateUnderMap();
	    output.setText("\n  Good Job!\n");
	    output.append("  Now that all your troops are ready to go, use the buttons at the bottom to defeat your Enemy!\n  This concludes the tutorial, Good luck Commander!\n\n");
	    showCurrentTurn();
	    
	    
	}
	
	private void tutorialTrain(){
		int[][] matrix;
		inBattle=true;
		setBattleParams();
		matrix = troopSpawn(5,0,0,0);
		output.setText("\n  In Training you will fight against tribes of Savages, which are weak, but in numbers they cause damage.\n");
		output.append("  Start any fight by moving your units into range, then attack.\n  Set up your units where you want to quickly wipe out the Savages!\n\n");
		repaint();
		matrix=chooseLocation(matrix);
		getActions();
		updateUnderMap();
		output.setText("\n  Good Job!\n");
	    output.append("  Now that all your troops are ready to go, use the buttons at the bottom to defeat your Enemy!\n\n");
	    showCurrentTurn();
	}
	////////////////////////////////////////TROOP SPAWN
	private int[][] troopSpawn(int s, int k, int c, int a){
		enemyUnits = new HashMap<String,Unit>();
		connections = new HashMap<String,Point>();
		turn = new ArrayList<String>();
		int[][] matrix = { 		  /*1*/   /*2*/   /*3*/   /*4*/   /*5*/   /*6*/   /*7*/   /*8*/   /*9*/
			/*1*/			{ 0, 0, 0, 0, 0, 0, 0, 0, 0},
			/*2*/			{ 0, 0, 0, 0, 0, 0, 0, 0, 0},
			/*3*/			{ 0, 0, 0, 0, 0, 0, 0, 0, 0},
			/*4*/			{ 0, 0, 0, 0, 0, 0, 0, 0, 0},
			/*5*/			{ 0, 0, 0, 0, 0, 0, 0, 0, 0},
			/*6*/			{ 0, 0, 0, 0, 0, 0, 0, 0, 0},
			/*7*/			{ 0, 0, 0, 0, 0, 0, 0, 0, 0},
			/*8*/			{ 0, 0, 0, 0, 0, 0, 0, 0, 0},
			/*9*/			{ 0, 0, 0, 0, 0, 0, 0, 0, 0},
			/*10*/			{ 0, 0, 0, 0, 0, 0, 0, 0, 0},
			/*11*/			{ 0, 0, 0, 0, 0, 0, 0, 0, 0},
			/*12*/			{ 0, 0, 0, 0, 0, 0, 0, 0, 0},
			/*13*/			{ 0, 0, 0, 0, 0, 0, 0, 0, 0},
			/*14*/			{ 0, 0, 0, 0, 0, 0, 0, 0, 0},
			/*15*/			{ 0, 0, 0, 0, 0, 0, 0, 0, 0}
								};
		int row = 0;
		int col = 0;
		for(int x=0;x<s;x++){
			row = (int)(Math.random()*3);
			col = (int)(Math.random()*9);
			while(matrix[row][col]!=0){
				row = (int)(Math.random()*3);
				col = (int)(Math.random()*9);
			}
			matrix[row][col] = 13;
			temp = getRandomName();
			enemyUnits.put(temp,new Unit(temp,"s"));
			connections.put(temp,new Point(row,col));
			turn.add(temp);
	//		System.out.println(connections.get(temp));
		}
		for(int x=0;x<k;x++){
			row = (int)(Math.random()*3);
			col = (int)(Math.random()*9);
			while(matrix[row][col]!=0){
				row = (int)(Math.random()*3);
				col = (int)(Math.random()*9);
			}
			matrix[row][col] = 1;
			temp = getRandomName();
			enemyUnits.put(temp,new Unit(temp,"ek"));
			connections.put(temp,new Point(row,col));
			turn.add(temp);
		}
		for(int x=0;x<c;x++){
			row = (int)(Math.random()*3);
			col = (int)(Math.random()*9);
			while(matrix[row][col]!=0){
				row = (int)(Math.random()*3);
				col = (int)(Math.random()*9);
			}
			matrix[row][col] = 5;
			temp = getRandomName();
			enemyUnits.put(temp,new Unit(temp,"ec"));
			connections.put(temp,new Point(row,col));
			turn.add(temp);
		}
		for(int x=0;x<a;x++){
			row = (int)(Math.random()*3);
			col = (int)(Math.random()*9);
			while(matrix[row][col]!=0){
				row = (int)(Math.random()*3);
				col = (int)(Math.random()*9);
			}
			matrix[row][col] = 3;
			temp = getRandomName();
			enemyUnits.put(temp,new Unit(temp,"ea"));
			connections.put(temp,new Point(row,col));
			turn.add(temp);
		}
		board = matrix;
		return matrix;
	}
	
	//////////////////////////////////////// INVENTORY
	public void inventoryOne(){
		
		weaponSwap=!weaponSwap;
	}
	public void inventoryTwo(){
		setParams();
		fightUI=true;
		invUI=false;
		
		buttonThree.setVisible(true);
		buttonFour.setVisible(true);
		data.setVisible(true);
		dataLabel.setVisible(true);
		scroll.setVisible(true);
		test.setRows(44);
		repaint();
	}
	public void swapMenu(int slot){ 
		if(slot<friendlyUnits.size()){
			ArrayList<String> temp = new ArrayList<String>();
			ArrayList<Integer> temp2 = new ArrayList<Integer>();
			temp.add("Basic");
			for(int x=0;x<allWeapons.size();x++){
				if(allWeapons.get(x).getType().equals(invUnits.get(slot).getWeapon().getType())){
					temp.add(""+allWeapons.get(x).getRarity()+" "+allWeapons.get(x).getType()+" ("+allWeapons.get(x).minDPS+" - "+allWeapons.get(x).maxDPS+")");
					temp2.add(x);
				}
					
					
			}
			temp.add("Cancel");
			int choice = JOptionPane.showOptionDialog(null,
	     		 "What weapon would you like to give this Unit?", 
	     		 "Replace Weapon", 
	     		 0, 
	     		 JOptionPane.QUESTION_MESSAGE, 
	     		 null, temp.toArray(), null);
	     	try {
	 		   if(choice == 0){
		     		if(!invUnits.get(slot).getWeapon().getRarity().equals("Basic"))
			     			allWeapons.add(invUnits.get(slot).getWeapon());
			     	invUnits.get(slot).basicWeapon();
		     	}
		     	else{
		     		if(choice<=allWeapons.size()){
			     		if(!invUnits.get(slot).getWeapon().getRarity().equals("Basic"))
			     			allWeapons.add(invUnits.get(slot).getWeapon());
			     		invUnits.get(slot).newWeapon(allWeapons.remove((int)temp2.get(choice-1)));
			     	}
		     	}
		     	weapons.clear();
				for(Map.Entry<String, Unit> element : friendlyUnits.entrySet()){
					weapons.add(element.getValue().getWeaponImageName());
					invUnits.add(element.getValue());
				}
				repaint();
				 
			}
			catch (Exception ex) {}
	     	 	
		}
		else
			JOptionPane.showMessageDialog(null,"There is no unit here!","Invalid Location",JOptionPane.INFORMATION_MESSAGE);
		weaponSwap=false;
		
	}
	public void showStats(int slot){ 
		if(slot<invUnits.size()&&invUnits.get(slot)!=null){
		 	JOptionPane.showMessageDialog(null,""+invUnits.get(slot).toString(),"Unit Stats",JOptionPane.INFORMATION_MESSAGE);
		}
		
	}
	public ArrayList<String> getInvUnitNames(){
		ArrayList<String> temp = new ArrayList<String>();
		for(Map.Entry<String, Unit> element : friendlyUnits.entrySet()){
			temp.add(element.getValue().getName());
		}
		return temp;
	}
	public ArrayList<String> getInvUnitTypes(){
		ArrayList<String> temp = new ArrayList<String>();
		for(Map.Entry<String, Unit> element : friendlyUnits.entrySet()){
			temp.add(element.getValue().getRealType());
		}
		return temp;
	}
	public ArrayList<String> getInvUnitWeapons(){
		ArrayList<String> temp = new ArrayList<String>();
		return temp;
	}
	public void addWeapon(){
		double odds=Math.random();
		String rare = "";
		String type = "";
		invUnits.clear();
		for(Map.Entry<String, Unit> element : friendlyUnits.entrySet()){
			invUnits.add(element.getValue());
		}
			if(Math.random()<=chanceOfFindingWeapon){
				if(odds<=.6)
					rare = "Rare";
				else
					if(odds<=.9)
						rare = "Epic";
					else
						rare="Legendary";
				odds=Math.random()*invUnits.size();
				type = invUnits.get((int)odds).getWeapon().getType();
				Weapon added = new Weapon(type,rare);
				allWeapons.add(added);
				JOptionPane.showMessageDialog(null,""+added.toString(),"New Weapon",JOptionPane.INFORMATION_MESSAGE);
			}
	}
	
	////////////////////////////////////////BASIC USE ITEMS
	public void delay(int sec){
		try {
		    Thread.sleep(sec);
		} catch(InterruptedException ex) {
		    Thread.currentThread().interrupt();
		}
	}
	private String findName(){
		String name = "";
		int range = (int)(Math.random()*names.size());
		name = names.remove(range);
		return name;
	}
	
	@Override
	public void setParams()
	{
		buttonOne.setText("Battle");
		buttonTwo.setText("Train");
		buttonThree.setText("Inventory");
		buttonFour.setText("Hire a Soldier");
		data.setText(""+cash);
		dataLabel.setText("Gold:");
	}
	public void setBattleParams()
	{
		buttonOne.setText("EMPTY");
		buttonTwo.setText("EMPTY");
		buttonThree.setText("End Turn");
		buttonFour.setText("Retreat");
		data.setText(""+cash);
		dataLabel.setText("Gold:");
	}
	
	public void setInventoryParams()
	{
		buttonOne.setText("Swap Weapon");
		buttonTwo.setText("Back");
	}
	
	public void setUnitParams(String name){
		if(friendlyUnits.get(name).hasSpellOne())
			buttonOne.setText(friendlyUnits.get(name).getSpellOne()+" - "+friendlyUnits.get(name).getSpellOneUses());
		else
			buttonOne.setText("EMPTY");
			
		if(friendlyUnits.get(name).hasSpellOne())
			buttonTwo.setText(friendlyUnits.get(name).getSpellTwo()+" - "+friendlyUnits.get(name).getSpellTwoUses());
		else
			buttonTwo.setText("EMPTY");
	}
	
	private void getActions(){
		String name = turn.get(0);
		if(friendlyUnits.containsKey(turn.get(0)))
			actions = friendlyUnits.get(name).getSpeed(); 
		else
			actions = enemyUnits.get(name).getSpeed(); 
	}
	
	private String getRandomName(){
		String randomName ="";
		char rdm = 'A';
		for(int x=0;x<10;x++){
				rdm = (char)((Math.random()*26)+ 65);
			randomName+=rdm;
		}
		return randomName;
	}
	
	private void healUnits(){
		for(Map.Entry<String, Unit> element : friendlyUnits.entrySet()){
			element.getValue().heal();
		}
	}
	private void resetAllSpells(){
		for(Map.Entry<String, Unit> element : friendlyUnits.entrySet()){
			element.getValue().resetSpells();
		}
	}
	
	private void resetBoard(){
		int[][] temp ={ 		
			/*1*/			{ 0, 0, 0, 0, 0, 0, 0, 0, 0},
			/*2*/			{ 0, 0, 0, 0, 0, 0, 0, 0, 0},
			/*3*/			{ 0, 0, 0, 0, 0, 0, 0, 0, 0},
			/*4*/			{ 0, 0, 0, 0, 0, 0, 0, 0, 0},
			/*5*/			{ 0, 0, 0, 0, 0, 0, 0, 0, 0},
			/*6*/			{ 0, 0, 0, 0, 0, 0, 0, 0, 0},
			/*7*/			{ 0, 0, 0, 0, 0, 0, 0, 0, 0},
			/*8*/			{ 0, 0, 0, 0, 0, 0, 0, 0, 0},
			/*9*/			{ 0, 0, 0, 0, 0, 0, 0, 0, 0},
			/*10*/			{ 0, 0, 0, 0, 0, 0, 0, 0, 0},
			/*11*/			{ 0, 0, 0, 0, 0, 0, 0, 0, 0},
			/*12*/			{ 0, 0, 0, 0, 0, 0, 0, 0, 0},
			/*13*/			{ 0, 0, 0, 0, 0, 0, 0, 0, 0},
			/*14*/			{ 0, 0, 0, 0, 0, 0, 0, 0, 0},
			/*15*/			{ 0, 0, 0, 0, 0, 0, 0, 0, 0}
								};
		board = temp;
	}
	private void updateUnderMap(){
		int[][] temp ={ 	
			/*1*/			{ 0, 0, 0, 0, 0, 0, 0, 0, 0},
			/*2*/			{ 0, 0, 0, 0, 0, 0, 0, 0, 0},
			/*3*/			{ 0, 0, 0, 0, 0, 0, 0, 0, 0},
			/*4*/			{ 0, 0, 0, 0, 0, 0, 0, 0, 0},
			/*5*/			{ 0, 0, 0, 0, 0, 0, 0, 0, 0},
			/*6*/			{ 0, 0, 0, 0, 0, 0, 0, 0, 0},
			/*7*/			{ 0, 0, 0, 0, 0, 0, 0, 0, 0},
			/*8*/			{ 0, 0, 0, 0, 0, 0, 0, 0, 0},
			/*9*/			{ 0, 0, 0, 0, 0, 0, 0, 0, 0},
			/*10*/			{ 0, 0, 0, 0, 0, 0, 0, 0, 0},
			/*11*/			{ 0, 0, 0, 0, 0, 0, 0, 0, 0},
			/*12*/			{ 0, 0, 0, 0, 0, 0, 0, 0, 0},
			/*13*/			{ 0, 0, 0, 0, 0, 0, 0, 0, 0},
			/*14*/			{ 0, 0, 0, 0, 0, 0, 0, 0, 0},
			/*15*/			{ 0, 0, 0, 0, 0, 0, 0, 0, 0}
								};
		 String name = turn.get(0);
		 if(name.contains(" ")){
		 	int range = friendlyUnits.get(name).getRange(); 
					
			Point location = new Point();
				location = connections.get(name);
			switch(friendlyUnits.get(name).getType()){
				case "k":
				for(int x=0;x<=range*2;x++){
					if((location.x+range-x<=14&&location.x+range-x>=0)&&location.x+range-x!=location.x){
						temp[location.x+range-x][location.y]++;
					}
				}
				for(int y=0;y<=range*2;y++){
					if((location.y+range-y<=8&&location.y+range-y>=0)&&location.y+range-y!=location.y){
						temp[location.x][location.y+range-y]++;
					}
				}
				break;
				case "c":
				for(int x=0;x<=range*2;x++){
					if(((location.x+range-x<=14&&location.x+range-x>=0)&&location.x+range-x!=location.x)&&
						Math.abs((location.x+range-x)-location.x+range)!=3&&Math.abs((location.x+range-x)-location.x+range)!=1){
						temp[location.x+range-x][location.y]++;
					}
				}
				for(int y=0;y<=range*2;y++){
					if(((location.y+range-y<=8&&location.y+range-y>=0)&&location.y+range-y!=location.y)&&
						Math.abs((location.y+range-y)-location.y+range)!=3&&Math.abs((location.y+range-y)-location.y+range)!=1){
						temp[location.x][location.y+range-y]++;
					}
				}
				break;
				case "w":
				for(int x=0;x<=range*2;x++){
					if(((location.x+range-x<=14&&location.x+range-x>=0)&&location.x+range-x!=location.x)&&
						Math.abs((location.x+range-x)-location.x+range)!=2&&Math.abs((location.x+range-x)-location.x+range)!=4){
						temp[location.x+range-x][location.y]++;
					}
				}
				for(int y=0;y<=range*2;y++){
					if(((location.y+range-y<=8&&location.y+range-y>=0)&&location.y+range-y!=location.y)&&
						Math.abs((location.y+range-y)-location.y+range)!=2&&Math.abs((location.y+range-y)-location.y+range)!=4){
						temp[location.x][location.y+range-y]++;
					}
				}
				break;
				case "a":
				for(int x=0;x<=range*2;x++){
					if(((location.x+range-x<=14&&location.x+range-x>=0)&&location.x+range-x!=location.x)&&
						Math.abs((location.x+range-x)-location.x+range)!=5&&Math.abs((location.x+range-x)-location.x+range)!=9&&
						Math.abs((location.x+range-x)-location.x+range)!=6&&Math.abs((location.x+range-x)-location.x+range)!=10&&
						Math.abs((location.x+range-x)-location.x+range)!=7&&Math.abs((location.x+range-x)-location.x+range)!=11){
						temp[location.x+range-x][location.y]++;
					}
				}
				for(int y=0;y<=range*2;y++){
					if(((location.y+range-y<=8&&location.y+range-y>=0)&&location.y+range-y!=location.y)&&
						Math.abs((location.y+range-y)-location.y+range)!=5&&Math.abs((location.y+range-y)-location.y+range)!=9&&
						Math.abs((location.y+range-y)-location.y+range)!=6&&Math.abs((location.y+range-y)-location.y+range)!=10&&
						Math.abs((location.y+range-y)-location.y+range)!=7&&Math.abs((location.y+range-y)-location.y+range)!=11){
						temp[location.x][location.y+range-y]++;
					}
				}
				break;
			}
			
			
			for(int x=0;x<=actions*2;x++){
				if((location.x+actions-x<=14&&location.x+actions-x>=0)&&location.x+actions-x!=location.x){
					temp[location.x+actions-x][location.y]+=2;
				}
			}
			for(int y=0;y<=actions*2;y++){
				if((location.y+actions-y<=8&&location.y+actions-y>=0)&&location.y+actions-y!=location.y){
					temp[location.x][location.y+actions-y]+=2;
				}
			}
			underMap=temp;
		 }
		 else{
		 	resetUnderMap();
		 }
		 
	}
	public void resetUnderMap(){
		int[][] temp ={ 	
			/*1*/			{ 0, 0, 0, 0, 0, 0, 0, 0, 0},
			/*2*/			{ 0, 0, 0, 0, 0, 0, 0, 0, 0},
			/*3*/			{ 0, 0, 0, 0, 0, 0, 0, 0, 0},
			/*4*/			{ 0, 0, 0, 0, 0, 0, 0, 0, 0},
			/*5*/			{ 0, 0, 0, 0, 0, 0, 0, 0, 0},
			/*6*/			{ 0, 0, 0, 0, 0, 0, 0, 0, 0},
			/*7*/			{ 0, 0, 0, 0, 0, 0, 0, 0, 0},
			/*8*/			{ 0, 0, 0, 0, 0, 0, 0, 0, 0},
			/*9*/			{ 0, 0, 0, 0, 0, 0, 0, 0, 0},
			/*10*/			{ 0, 0, 0, 0, 0, 0, 0, 0, 0},
			/*11*/			{ 0, 0, 0, 0, 0, 0, 0, 0, 0},
			/*12*/			{ 0, 0, 0, 0, 0, 0, 0, 0, 0},
			/*13*/			{ 0, 0, 0, 0, 0, 0, 0, 0, 0},
			/*14*/			{ 0, 0, 0, 0, 0, 0, 0, 0, 0},
			/*15*/			{ 0, 0, 0, 0, 0, 0, 0, 0, 0}
								};
		underMap=temp;
		repaint();
	}
	public void resetTerrainMap(){
		int[][] temp = { 	
			/*1*/			{ 0, 0, 0, 0, 0, 0, 0, 0, 0},
			/*2*/			{ 0, 0, 0, 0, 0, 0, 0, 0, 0},
			/*3*/			{ 0, 0, 0, 0, 0, 0, 0, 0, 0},
			/*4*/			{ 0, 0, 0, 0, 0, 0, 0, 0, 0},
			/*5*/			{ 0, 0, 0, 0, 0, 0, 0, 0, 0},
			/*6*/			{ 0, 0, 0, 0, 0, 0, 0, 0, 0},
			/*7*/			{ 0, 0, 0, 0, 0, 0, 0, 0, 0},
			/*8*/			{ 0, 0, 0, 0, 0, 0, 0, 0, 0},
			/*9*/			{ 0, 0, 0, 0, 0, 0, 0, 0, 0},
			/*10*/			{ 0, 0, 0, 0, 0, 0, 0, 0, 0},
			/*11*/			{ 0, 0, 0, 0, 0, 0, 0, 0, 0},
			/*12*/			{ 0, 0, 0, 0, 0, 0, 0, 0, 0},
			/*13*/			{ 0, 0, 0, 0, 0, 0, 0, 0, 0},
			/*14*/			{ 0, 0, 0, 0, 0, 0, 0, 0, 0},
			/*15*/			{ 0, 0, 0, 0, 0, 0, 0, 0, 0}
								};
		terrain=temp;
		repaint();
	}

	@Override
	public void run()
	{
		
		data.setText(""+1000);
  //		int choice = JOptionPane.showOptionDialog(null, 
 //    	 "Would you like to do the Tutorial?", 
   //  	 "Tutorial", 
     //	 JOptionPane.YES_NO_OPTION, 
     //	 JOptionPane.QUESTION_MESSAGE, 
     //	 null, null, null);
     int choice =0;
     	if(choice==0){
     		tutorial=1;
     		output.setText( format("%2s%s","","\n  Welcome to the Tutorial!\n  First, your gonna need an army..\n  Try buying some troops using the button in the bottom right.\n\n") );
     	}
	    else
	    	output.setText( format("\n") );
	}
	
	@Override
	public void saveGame(){
		JOptionPane.showMessageDialog(null,"This feature is not in the game yet!","Save",JOptionPane.INFORMATION_MESSAGE);
	}
	@Override
	public void loadGame(){
		JOptionPane.showMessageDialog(null,"This feature is not in the game yet!","Load",JOptionPane.INFORMATION_MESSAGE);
	}
	@Override
	public void exitGame(){
			String[] list = {"Yes! Im done for now!","No, I misclicked.."};
			int choice = JOptionPane.showOptionDialog(null, 
	     		 "Do you really want to leave the game?", 
	     		 "Exit", 
	     		 0, 
	     		 JOptionPane.QUESTION_MESSAGE, 
	     		 null, list, null);
	     	if(choice ==0){
	     		System.exit(0);
	     	}
	}


	
}



abstract class View extends JFrame 
{
	JTextArea  output,test;
	JButton    buttonOne, buttonTwo, buttonThree, buttonFour;
	JTextField input, data;
	JLabel     inputLabel, dataLabel;
	JLabel     spaceThree, spaceFour;
	JScrollPane scroll;
	public int goodAlive, badAlive;
	/////TESTING BELOW
	protected int os      = 50;	
	protected int numRows = 15;
	protected int numCols = 9;
	protected int width   = numCols*os+14;
	protected int height  = numRows*os+36;
	ArrayList<ImageIcon> landscapes = new ArrayList<ImageIcon>();
	int realX, realY;
	boolean inrange2;
	
	boolean spellOneOn, spellTwoOn, usedSpell, weaponSwap, inBattle;
	int spellRange, inventorySlot;
	
	/*  EXPERIMENT! */
	boolean fightUI = true;
	boolean invUI= false;
	ArrayList<String> weapons = new ArrayList<String>();
	
	ArrayList<String> unitNames, unitTypes, unitWeapons;
	
	int[][] underMap = { 	
			/*1*/			{ 0, 0, 0, 0, 0, 0, 0, 0, 0},
			/*2*/			{ 0, 0, 0, 0, 0, 0, 0, 0, 0},
			/*3*/			{ 0, 0, 0, 0, 0, 0, 0, 0, 0},
			/*4*/			{ 0, 0, 0, 0, 0, 0, 0, 0, 0},
			/*5*/			{ 0, 0, 0, 0, 0, 0, 0, 0, 0},
			/*6*/			{ 0, 0, 0, 0, 0, 0, 0, 0, 0},
			/*7*/			{ 0, 0, 0, 0, 0, 0, 0, 0, 0},
			/*8*/			{ 0, 0, 0, 0, 0, 0, 0, 0, 0},
			/*9*/			{ 0, 0, 0, 0, 0, 0, 0, 0, 0},
			/*10*/			{ 0, 0, 0, 0, 0, 0, 0, 0, 0},
			/*11*/			{ 0, 0, 0, 0, 0, 0, 0, 0, 0},
			/*12*/			{ 0, 0, 0, 0, 0, 0, 0, 0, 0},
			/*13*/			{ 0, 0, 0, 0, 0, 0, 0, 0, 0},
			/*14*/			{ 0, 0, 0, 0, 0, 0, 0, 0, 0},
			/*15*/			{ 0, 0, 0, 0, 0, 0, 0, 0, 0}
								};
	int[][] terrain = { 	
			/*1*/			{ 0, 0, 0, 0, 0, 0, 0, 0, 0},
			/*2*/			{ 0, 0, 0, 0, 0, 0, 0, 0, 0},
			/*3*/			{ 0, 0, 0, 0, 0, 0, 0, 0, 0},
			/*4*/			{ 0, 0, 0, 0, 0, 0, 0, 0, 0},
			/*5*/			{ 0, 0, 0, 0, 0, 0, 0, 0, 0},
			/*6*/			{ 0, 0, 0, 0, 0, 0, 0, 0, 0},
			/*7*/			{ 0, 0, 0, 0, 0, 0, 0, 0, 0},
			/*8*/			{ 0, 0, 0, 0, 0, 0, 0, 0, 0},
			/*9*/			{ 0, 0, 0, 0, 0, 0, 0, 0, 0},
			/*10*/			{ 0, 0, 0, 0, 0, 0, 0, 0, 0},
			/*11*/			{ 0, 0, 0, 0, 0, 0, 0, 0, 0},
			/*12*/			{ 0, 0, 0, 0, 0, 0, 0, 0, 0},
			/*13*/			{ 0, 0, 0, 0, 0, 0, 0, 0, 0},
			/*14*/			{ 0, 0, 0, 0, 0, 0, 0, 0, 0},
			/*15*/			{ 0, 0, 0, 0, 0, 0, 0, 0, 0}
								};

	public View()
	{ 
		addMouseListener( new MouseHandler() );
		
		landscapes.add(new ImageIcon( "grass2.jpg" ));
		landscapes.add(new ImageIcon( "grass21.jpg" ));
		landscapes.add(new ImageIcon( "grass22.jpg" ));
		landscapes.add(new ImageIcon( "grass23.jpg" ));
		landscapes.add(new ImageIcon( "grass2b.jpg" ));
		landscapes.add(new ImageIcon( "grass21b.jpg" ));
		landscapes.add(new ImageIcon( "grass22b.jpg" ));
		landscapes.add(new ImageIcon( "grass23b.jpg" ));
		
		landscapes.add(new ImageIcon( "grass2S.jpg" ));
		landscapes.add(new ImageIcon( "grass21S.jpg" ));
		landscapes.add(new ImageIcon( "grass22S.jpg" ));
		landscapes.add(new ImageIcon( "grass23S.jpg" ));
		landscapes.add(new ImageIcon( "grass2bS.jpg" ));
		landscapes.add(new ImageIcon( "grass21bS.jpg" ));
		landscapes.add(new ImageIcon( "grass22bS.jpg" ));
		landscapes.add(new ImageIcon( "grass23bS.jpg" ));
		
		setSize(width+350,height+160);
		setLocationRelativeTo(null);
  		setDefaultCloseOperation(3);
		setContentPane( new MyPanel() );
		
		setVisible(true);

		run();
	}
	class MouseHandler extends MouseAdapter
	{
		@Override
	 	public void mouseClicked  (MouseEvent event)
	 	{
	 		int xpos = event.getX();
	  		int ypos = event.getY();
	  		if(fightUI){
	  			realY = (xpos-8)/50;
	  			realX = (ypos-50)/50;
	  		}
	  		if(invUI){
	  			boolean validSpot = false;
	  			if(ypos<=315&&ypos>=230){
	  				if(xpos<=245&&xpos>=150){
	  					validSpot=true;
	  					inventorySlot=0;
	  				}
	  				if(xpos<=385&&xpos>=290){
	  					validSpot=true;
	  					inventorySlot=1;
	  				}
	  				if(xpos<=525&&xpos>=430){
	  					validSpot=true;
	  					inventorySlot=2;
	  				}
	  				if(xpos<=665&&xpos>=570){
	  					validSpot=true;
	  					inventorySlot=3;
	  				}
	  			}
	  			if(ypos<=475&&ypos>=388){
	  				if(xpos<=245&&xpos>=150){
	  					validSpot=true;
	  					inventorySlot=4;
	  				}
	  				if(xpos<=385&&xpos>=290){
	  					validSpot=true;
	  					inventorySlot=5;
	  				}
	  				if(xpos<=525&&xpos>=430){
	  					validSpot=true;
	  					inventorySlot=6;
	  				}
	  				if(xpos<=665&&xpos>=570){
	  					validSpot=true;
	  					inventorySlot=7;
	  				}
	  			}
	  			if(ypos<=635&&ypos>=547){
	  				if(xpos<=245&&xpos>=150){
	  					validSpot=true;
	  					inventorySlot=8;
	  				}
	  				if(xpos<=385&&xpos>=290){
	  					validSpot=true;
	  					inventorySlot=9;
	  				}
	  				if(xpos<=525&&xpos>=430){
	  					validSpot=true;
	  					inventorySlot=10;
	  				}
	  				if(xpos<=665&&xpos>=570){
	  					validSpot=true;
	  					inventorySlot=11;
	  				}
	  			}
	  			if(ypos<=790&&ypos>=702){
	  				if(xpos<=245&&xpos>=150){
	  					validSpot=true;
	  					inventorySlot=12;
	  				}
	  				if(xpos<=385&&xpos>=290){
	  					validSpot=true;
	  					inventorySlot=13;
	  				}
	  				if(xpos<=525&&xpos>=430){
	  					validSpot=true;
	  					inventorySlot=14;
	  				}
	  				if(xpos<=665&&xpos>=570){
	  					validSpot=true;
	  					inventorySlot=15;
	  				}
	  			}
	  		//	out.println(inventorySlot);
	  			if(weaponSwap&&validSpot){
	  				swapMenu(inventorySlot);
	  			}
	  			else{
	  				if(validSpot)
	  					showStats(inventorySlot);
	  			}
	  			
	  		}
	  		
		//	System.out.println("X - "+realX);
		// 	System.out.println("Y - "+realY);
			if(spellOneOn||spellTwoOn){
				if(spellOneOn)
					spellOneEffect(realX,realY);
				else
					spellTwoEffect(realX,realY);
			}
			else{
				if(realX<15&&realY<9){
			  		if(underMap[realX][realY]==1&&unitThere()){
			  			String[] list = {"Yes","No"};
						int choice = JOptionPane.showOptionDialog(null, 
				     		 "Do you want to attack this unit?", 
				     		 "Attack", 
				     		 0, 
				     		 JOptionPane.QUESTION_MESSAGE, 
				     		 null, list, null);
				     	if(choice ==0){
				     		chooseAttack2();
				     	}
			  		}
			  		else
				  		if(underMap[realX][realY]==3){
				  			if(checkAttack()){
				  				String[] list = {"Yes","No"};
								int choice = JOptionPane.showOptionDialog(null, 
						     		 "Do you want to attack this unit?", 
						     		 "Attack", 
						     		 0, 
						     		 JOptionPane.QUESTION_MESSAGE, 
						     		 null, list, null);
						     	if(choice ==0){
						     		chooseAttack2();
						     	}
				  			}
				  			else
				  				moveUnit2();
				  		}
					  		
					  	else
					  		if(underMap[realX][realY]==2)
					  			moveUnit2();
			  	}
			}	
	 	}
	}

	class MyPanel extends JPanel
	{
		MyPanel()
		{
			test = new JTextArea();
				test.setRows(44);
				test.setLineWrap(true);
				test.setWrapStyleWord(true);
	//			test.setColumns(5);
				test.setSize(1,50);
				test.setEditable(false);
				test.setFocusable(false);
				test.setFont( new java.awt.Font("DialogInput", 0, 12));
			test.setOpaque(false);
		//	test.setBackground(new Color(255,255,255,128));
			add(test);
   			setBackground(new Color(0,100,60));
   			MenuBar objMenuBar = new MenuBar();
	        Menu menu1 = new Menu("Menu");
		        MenuItem menuitemsave = new MenuItem("Save");
		        menu1.add(menuitemsave);
		        menu1.add("-");
		        MenuItem menuitemload = new MenuItem("Load");
		        menu1.add(menuitemload);
		        menu1.add("-");
		        MenuItem menuitemexit = new MenuItem("Exit");
		        menu1.add(menuitemexit);

	        objMenuBar.add(menu1);
	        setMenuBar( objMenuBar );

			
			
			output = new JTextArea();
				output.setRows(5);
				output.setLineWrap(true);
				output.setWrapStyleWord(true);
				output.setColumns(112);
				output.setLocation(500,500);
				output.setEditable(false);
				output.setFocusable(false);
				output.setFont( new java.awt.Font("DialogInput", 0, 12));
			scroll = new JScrollPane(output);
				scroll.setVerticalScrollBarPolicy(ScrollPaneConstants.VERTICAL_SCROLLBAR_ALWAYS);
				scroll.setHorizontalScrollBarPolicy(JScrollPane.HORIZONTAL_SCROLLBAR_NEVER);
			add(scroll);
			
			

			buttonOne = new JButton();
				buttonOne.setPreferredSize(new Dimension(120,25));
				buttonOne.addActionListener( new ActionOneListener() );
				buttonOne.setFocusable(false);
			add(buttonOne);

			buttonTwo = new JButton();
				buttonTwo.setPreferredSize(new Dimension(120,25));
				buttonTwo.addActionListener( new ActionTwoListener() );
				buttonTwo.setFocusable(false);
			add(buttonTwo);

			buttonThree = new JButton();
				buttonThree.setPreferredSize(new Dimension(120,25));
				buttonThree.addActionListener( new ActionThreeListener() );
				buttonThree.setFocusable(false);
			add(buttonThree);
			
		

			add( spaceThree = new JLabel() );
			
			inputLabel = new JLabel();
//				inputLabel.setPreferredSize(new Dimension(40,25));
				inputLabel.setHorizontalAlignment(4);
//			add(inputLabel);

			input = new JTextField();
				input.addActionListener( new InputFieldActionListener() );
				input.setHorizontalAlignment(JTextField.CENTER);
				input.setFont( new java.awt.Font("DialogInput", 1, 12));
				input.setPreferredSize( new Dimension( 120, 24 ) );
				input.setBackground( new Color(200,200,200) );
		//	add(input);

			add( spaceFour = new JLabel() );

			dataLabel = new JLabel();
//				dataLabel.setPreferredSize(new Dimension(40,25));
				dataLabel.setHorizontalAlignment(4);
				dataLabel.setForeground (Color.RED);
			add(dataLabel);

			data = new JTextField();
				data.setEditable(false);
				data.setFocusable(false);
				data.setHorizontalAlignment(JTextField.CENTER);
				data.setFont( new java.awt.Font("DialogInput", 1, 12));
				data.setPreferredSize( new Dimension( 90, 24 ) );
				data.setBackground( new Color(200,200,200) );
			add(data);
			
				buttonFour = new JButton();
				buttonFour.setPreferredSize(new Dimension(120,25));
				buttonFour.addActionListener( new ActionFourListener() );
				buttonFour.setFocusable(false);
			add(buttonFour);
			
			menuitemsave.addActionListener(new ActionListener() {
	            public void actionPerformed(ActionEvent actionevent) {
	                saveGame();
					repaint();
	        }});
	        menuitemload.addActionListener(new ActionListener() {
	            public void actionPerformed(ActionEvent actionevent) {
	                loadGame();
					repaint();
	        }});
	        menuitemexit.addActionListener(new ActionListener() {
	            public void actionPerformed(ActionEvent actionevent) {
	                exitGame();
	        }});
			setParams();
		}
		
		public void paintComponent(Graphics g)
		{
			Graphics2D g2 = (Graphics2D) g; 
			g2.setRenderingHint(
   		      RenderingHints.KEY_TEXT_ANTIALIASING,
   		      RenderingHints.VALUE_TEXT_ANTIALIAS_ON);
			super.paintComponent(g);
			if(fightUI){
				for(int r=0; r<numRows; r++)
				  for(int c=0; c<numCols; c++){
				  	if(underMap[r][c]==0){
				  		if(terrain[r][c]==0)
				  			g.drawImage(landscapes.get(0).getImage(),c*os,r*os,null);
				  		else
				  			if(terrain[r][c]==1)
				  				g.drawImage(landscapes.get(4).getImage(),c*os,r*os,null);
				  	}
				  		
				  	else{
				  		if(underMap[r][c]==1){
				  			if(terrain[r][c]==0)
					  			g.drawImage(landscapes.get(1).getImage(),c*os,r*os,null);
					  		else
					  			if(terrain[r][c]==1)
					  				g.drawImage(landscapes.get(5).getImage(),c*os,r*os,null);
				  		}
					  		
					  	else{
					  		if(underMap[r][c]==2){
					  			if(terrain[r][c]==0)
						  			g.drawImage(landscapes.get(2).getImage(),c*os,r*os,null);
						  		else
						  			if(terrain[r][c]==1)
						  				g.drawImage(landscapes.get(6).getImage(),c*os,r*os,null);
					  		}
						  	else{
						  		if(underMap[r][c]==3){
						  			if(terrain[r][c]==0)
								  		g.drawImage(landscapes.get(3).getImage(),c*os,r*os,null);
									else
								  		if(terrain[r][c]==1)
								  			g.drawImage(landscapes.get(7).getImage(),c*os,r*os,null);
						  		}
						  	else{
						  		if(underMap[r][c]==4){
						  			if(terrain[r][c]==0)
								  		g.drawImage(landscapes.get(8).getImage(),c*os,r*os,null);
									else
								  		if(terrain[r][c]==1)
								  			g.drawImage(landscapes.get(12).getImage(),c*os,r*os,null);
						  		}
						  	
						  	else{
						  		if(underMap[r][c]==5){
						  			if(terrain[r][c]==0)
								  		g.drawImage(landscapes.get(9).getImage(),c*os,r*os,null);
									else
								  		if(terrain[r][c]==1)
								  			g.drawImage(landscapes.get(13).getImage(),c*os,r*os,null);
						  		}
						  	
						  	else{
						  		if(underMap[r][c]==6){
						  			if(terrain[r][c]==0)
								  		g.drawImage(landscapes.get(10).getImage(),c*os,r*os,null);
									else
								  		if(terrain[r][c]==1)
								  			g.drawImage(landscapes.get(14).getImage(),c*os,r*os,null);
						  		}
						  	
						  	else{
						  		if(underMap[r][c]==7){
						  			if(terrain[r][c]==0)
								  		g.drawImage(landscapes.get(11).getImage(),c*os,r*os,null);
									else
								  		if(terrain[r][c]==1)
								  			g.drawImage(landscapes.get(15).getImage(),c*os,r*os,null);
						  		}
						  	}
						  	}
						  	}
						  	}
						  	} 		
					  	}		
				  	}	
				  }
				g.drawImage(new ImageIcon( "knight1.png" ).getImage(),500,50,null);
				g.drawImage(new ImageIcon( "archer1.png" ).getImage(),500,100,null);
				g.drawImage(new ImageIcon( "cav1.png" ).getImage(),500,150,null);
				g.drawImage(new ImageIcon( "knight2.png" ).getImage(),500,200,null);
				g.drawImage(new ImageIcon( "archer2.png" ).getImage(),500,250,null);
				g.drawImage(new ImageIcon( "cav2.png" ).getImage(),500,300,null);
				g.drawImage(new ImageIcon( "savage.png" ).getImage(),500,350,null);
				g.drawImage(new ImageIcon( "knight1x.png" ).getImage(),500,460,null);
				g.drawImage(new ImageIcon( "archer1x.png" ).getImage(),500,510,null);
				g.drawImage(new ImageIcon( "cav1x.png" ).getImage(),500,560,null);
				
				g.setFont(new Font("Times New Roman",1,18));
				g.setColor(Color.BLACK);
				g.drawString("Friendly Knight",570,75);
				g.drawString("Friendly Archer",570,125);
				g.drawString("Friendly Cavalry",570,175);
				g.drawString("Enemy Knight",570,225);
				g.drawString("Enemy Archer",570,275);
				g.drawString("Enemy Cavalry",570,325);
				g.drawString("Enemy Savage",570,375);
				g.drawString("Friendly Knight",570,485);
				g.drawString("Friendly Archer",570,535);
				g.drawString("Friendly Cavalry",570,585);
				
				
				g.setFont(new Font("Times New Roman",3,25));
				g.drawString(" --Current Turn--",520,435);
				g.drawString("     --Unit Types--",515,35);
				g.drawString("Allies Alive:         "+goodAlive,520,655);
				g.drawString("Enemies Alive:    "+badAlive,520,700);
				
	
				Graphics2D g2d = (Graphics2D)g;
				g2d.setStroke(new BasicStroke(2,1,0));
				g2d.setColor(new Color(0,0,0,50));
				for(int x=0; x<width; x+=os)
					g2d.drawLine(x,0,x,height-38);
				for(int y=0; y<height; y+=os)
					g2d.drawLine(0,y,width-13,y);
	
				drawPieces(g2d);
			}
			if(invUI){
				unitNames=getInvUnitNames();
				unitTypes=getInvUnitTypes();
				unitWeapons=getInvUnitWeapons();
				g.setFont(new Font("Times New Roman",3,25));
				g.drawString(" --Inventory--",340,50);
				g.drawImage(new ImageIcon( "inventory.png" ).getImage(),105,95,null);
				
				g.setFont(new Font("Times New Roman",3,15));
				g.setColor(Color.ORANGE);
				int temp = 0;
				int temp2 = 0;
				for(int x=0;x<unitNames.size();x++){
					temp =x;
					temp2=temp/4;
					temp%=4;
					if(unitNames.get(x).length()>14)
						g.drawString(""+unitNames.get(x).substring(0,14),temp*141+145,temp2*160+130);
					else
						g.drawString(""+unitNames.get(x),temp*141+145,temp2*160+130);
					g.drawString(""+unitTypes.get(x),temp*141+165,temp2*160+160);
					
					g.drawImage(new ImageIcon( weapons.get(x) ).getImage(),temp*141+148,temp2*160+185,null);
				}
				
				
			}
			
		}
		
	}
	
	
	class ActionOneListener implements ActionListener {
		public void actionPerformed(ActionEvent event) {
			if(fightUI)
				buttonOneAction();
			if(invUI)
				inventoryOne();
	}}

	public void buttonOneAction()
	{}
	public void inventoryOne(){}

	class ActionTwoListener implements ActionListener {
		public void actionPerformed(ActionEvent event) {
			if(fightUI)
				buttonTwoAction();
			if(invUI)
				inventoryTwo();
	}}
	
	public void buttonTwoAction()
	{}
	public void inventoryTwo(){}

	class ActionThreeListener implements ActionListener {
		public void actionPerformed(ActionEvent event) {
			buttonThreeAction();
	}}

	public void buttonThreeAction()
	{}

	class ActionFourListener implements ActionListener {
		public void actionPerformed(ActionEvent event) {
			buttonFourAction();
	}}

	public void buttonFourAction()
	{}

	class InputFieldActionListener implements ActionListener {
		public void actionPerformed(ActionEvent event) {
			inputFieldAction();
	}}

	public void inputFieldAction()
	{}

	public void	setParams()
	{
	}

	public void run()
	{
	}
	public void saveGame(){
		
	}
	public void loadGame(){
		
	}
	public void exitGame(){
		
	}
	public void moveUnit2(){
		
	}
	public boolean unitThere(){
		return false;
	}
	public void spellOneEffect(int xPos,int yPos){}
	public void spellTwoEffect(int xPos,int yPos){}
	
	public boolean checkAttack(){return false;}
	public void chooseAttack2(){
		
	}
	public void swapMenu(int slot){}
	public void showStats(int slot){}
	
	public ArrayList<String> getInvUnitNames(){return null;}
	public ArrayList<String> getInvUnitTypes(){return null;}
	public ArrayList<String> getInvUnitWeapons(){return null;}
	
	public void delay(double sec)
	{
		try {
		    Thread.sleep((int)(100*sec));
		} catch(InterruptedException ex) {
		    Thread.currentThread().interrupt();
		}
	}
	abstract protected void drawPieces(Graphics2D g);
}


////////////////////////////////////////UNIT CLASS BELOW
class Unit{
	private String name;
	private String type;
	private int hp;
	private int maxhp;
	private int minRange;
	private int maxRange;
	private int speed;
	
	private int spellOneUses;
	private int maxSpellOneUses;
	private int spellTwoUses;
	private int maxSpellTwoUses;
	
	private String spellOneName = "EMPTY";
	private String spellTwoName = "EMPTY";
	Weapon wep;
	
	public Unit(String n,String t){
		type = t;
		name=n;
		
		if(type.equals("k")||type.equals("ek")){
			wep = new Weapon("Sword","Basic");
			maxRange =1;
			speed =2;
		}
		if(type.equals("c")||type.equals("ec")){
			wep = new Weapon("Lance","Basic");
			maxRange =2;
			speed =8;
		}
		if(type.equals("a")||type.equals("ea")){
			wep = new Weapon("Bow","Basic");
			maxRange =8;
			speed =3;
		}
		if(type.equals("w")){
			wep = new Weapon("Staff","Basic");
			maxRange =3;
			speed =2;
			spellOneName= "Fire Strike";
			spellTwoName= "Fire Spread";
			maxSpellOneUses = 1;
			maxSpellTwoUses = 2;
		}
		if(type.equals("s")){
			wep = new Weapon("Axe","Basic");
			maxRange =1;
			speed =4;
		}
		resetSpells();
		maxhp=setMaxhp();
		hp=maxhp;
	}
	
	public void heal(){
		hp = maxhp;
	}
	
	private int setMaxhp(){
		if(type.equals("k")){
			return (int)((Math.random()*101)+ 200);
		}
		if(type.equals("c")){
			return (int)((Math.random()*250)+ 250);
		}
		if(type.equals("a")){
			return (int)((Math.random()*51)+ 50);
		}
		if(type.equals("w")){
			return (int)((Math.random()*51)+ 50); 
		}
		if(type.equals("s")){
			return (int)((Math.random()*31)+ 50);
		}
		return 0;
	}
	
	public String getName(){
		return name;
	}
	public String getType(){
		return type;
	}
	public int getHP(){
		return hp;
	}
	public int getRange(){
		return maxRange;
	}
	public int getSpeed(){
		return speed;
	}
	public int getDPS(){
		return wep.getDPS();
	}
	
	
	public boolean damage(int x){
		if(hp-x<=0)
			return false;
		else 
			hp-=x;
		return true;
	}
	
	public String getRealType(){
		String realType = "";
		switch(type){
			case "k": realType="Knight"; break;
			case "c": realType="Cavalry"; break;
			case "a": realType="Archer"; break;
			case "ek": realType="Enemy Knight"; break;
			case "ec": realType="Enemy Cavalry"; break;
			case "ea": realType="Enemy Archer"; break;
			case "s": realType="Savage"; break;
			case "w": realType="Wizard"; break;
		};
		return realType;
	}
	
	public boolean hasSpellOne(){
		if(spellOneName.equals("EMPTY"))
			return false;
		else
			return true;
	}
	public boolean hasSpellTwo(){
		if(spellTwoName.equals("EMPTY"))
			return false;
		else
			return true;
	}
	public String getSpellOne(){
		return spellOneName;
	}
	public String getSpellTwo(){
		return spellTwoName;
	}
	public void resetSpells(){
		spellOneUses = maxSpellOneUses;
		spellTwoUses = maxSpellTwoUses;
	}
	public boolean useSpellOne(){
		if(spellOneUses>0){
			spellOneUses--;
			return true;
		}
		else
			return false;	
	}
	public boolean useSpellTwo(){
		if(spellTwoUses>0){
			spellTwoUses--;
			return true;
		}
		else
			return false;	
	}
	public int getSpellOneUses(){
		return spellOneUses;
	}
	public int getSpellTwoUses(){
		return spellTwoUses;
	}
	public void newWeapon(Weapon w){
		wep = w;
	}
	public void basicWeapon(){
		wep=new Weapon(wep.getType(),"Basic");
	}
	public String getWeaponImageName(){
		return wep.getWeaponImageName();
	}
	public Weapon getWeapon(){
		return wep;
	}
	@Override
	public String toString(){
		String realType = getRealType();
		return "  Name: "+name+"\n  Unit Type: "+realType+"\n  Current Health: "+hp+"\n  Damage: "+wep.minDPS+" - "+wep.maxDPS+"\n  Range: "+minRange+" - "+ maxRange+"\n  Speed: "+speed+"\n\n";
	}
}

//////////////////////// WEAPON CLASS

class Weapon{
	private String type;
	private String rarity;
	public int maxDPS;
	public int minDPS;
	private String weaponImageName;
	
	public Weapon(String t,String r){
		type = t;
		rarity=r;
		
		if(type.equals("Sword")){
			minDPS=50;
		}
		if(type.equals("Lance")){
			minDPS=150;
		}
		if(type.equals("Bow")){
			minDPS=25;
		}
		if(type.equals("Staff")){
			minDPS=50;
		}
		if(type.equals("Axe")){
			minDPS=30;
		}
		weaponImageName = type;
		maxDPS=setDPS();
		makeRarityChanges();
	}
	
	private int setDPS(){
		if(type.equals("Sword")){
			return 100;
		}
		if(type.equals("Lance")){
			return 300;
		}
		if(type.equals("Bow")){
			return 125;
		}
		if(type.equals("Staff")){
			return 100;
		}
		if(type.equals("Axe")){
			return (int)((Math.random()*131)+ 30);
		}
		return 0;
		
	}
	private void makeRarityChanges(){
		if(rarity.equals("Rare")){
			minDPS=(int)(minDPS*1.3);
			maxDPS=(int)(maxDPS*((int)((Math.random()*3)+ 11))/10.0);
			weaponImageName+="R";
		}
		if(rarity.equals("Epic")){
			minDPS=(int)(minDPS*1.6);
			maxDPS=(int)(maxDPS*((int)((Math.random()*3)+ 14))/10.0);
			weaponImageName+="E";
		}
		if(rarity.equals("Legendary")){
			minDPS=(int)(minDPS*2);
			maxDPS=(int)(maxDPS*((int)((Math.random()*3)+ 18))/10.0);
			weaponImageName+="L";
		}
	}
	public String getType(){
		return type;
	}
	public String getRarity(){
		return rarity;
	}
	public int getDPS(){
		return (int)((Math.random()*(maxDPS-minDPS+1))+ minDPS);
	}
	public String getWeaponImageName(){
		return weaponImageName+".png";
	}
	@Override
	public String toString(){
		return "  You found a "+rarity+" "+type+"! This weapon does "+minDPS+" - "+maxDPS+" damage.";
	}
}
