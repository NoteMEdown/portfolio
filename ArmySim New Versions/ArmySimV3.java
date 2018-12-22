/**
 * @(#)ArmySimV3.java
 *
 *GOAL: UPDATE ARMY SIM WITH MORE USER FRIENDLY MOVING AND ATTACKING
 *
 * @author Nathan Frazier
 * @version 3.99 2015/12/16
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


public class ArmySimV3 {
    public static void main(String[] args) {
    	new Environment();
    }
}

class Environment extends View
{
	protected int pad = 2;
	ArrayList<ImageIcon> icons = new ArrayList<ImageIcon>();

	int[][] board = { 		  /*1*/   /*2*/   /*3*/   /*4*/   /*5*/   /*6*/   /*7*/   /*8*/   /*9*/
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
	
	HashMap<String,Knight> soldiers;
	HashMap<String,Cavalry> cavalry;
	HashMap<String,Archer> archers;
	
	HashMap<String,Knight> enemySoldiers;
	HashMap<String,Cavalry> enemyCavalry;
	HashMap<String,Archer> enemyArchers;
	HashMap<String,Savage> enemySavages;
	
	ArrayList<String> names;
	Scanner scan;
	private int cash=1000;
	private int tutorial;
	private boolean inBattle;
	String temp;
	
	int armySize;
	
	private boolean enemyMoved;
	private boolean enemyAttacked;
	
	HashMap<String,Point> connections;
	ArrayList<String> turn;
	private int actions;
	
	boolean win;
	boolean lose;
	boolean turn1;
	
	public Environment()
	{
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
		soldiers = new HashMap<String,Knight>();
		cavalry = new HashMap<String,Cavalry>();
		archers = new HashMap<String,Archer>();
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
	public void buttonOneAction()  //////// BATTLE &&& ATTACK
	{
		if(inBattle){
				/// THIS DOES NOTHING RIGHT NOW!
		}
		else{
			checkArmySize();
			if (armySize==0){
				JOptionPane.showMessageDialog(null,"You need units before you can fight anyone!","No Units",JOptionPane.WARNING_MESSAGE);
			}
			else{
				if(tutorial==0){
					inBattle=true;
					setBattleParams();
					int[][] matrix;
					matrix = troopSpawn(0,soldiers.size(),cavalry.size(),archers.size());
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
	public void buttonTwoAction()  //////// TRAIN &&& MOVE
	{
		if(inBattle){
			/// THIS DOES NOTHING RIGHT NOW!
		}
		else{
			checkArmySize();
			if (armySize==0){
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
				resetBoard();
				resetUnderMap();
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
				for(Map.Entry<String, Archer> element : archers.entrySet()){
					element.getValue().heal();
				}
				for(Map.Entry<String, Knight> element : soldiers.entrySet()){
					element.getValue().heal();
				}
				for(Map.Entry<String, Cavalry> element : cavalry.entrySet()){
					element.getValue().heal();
				}
			}
		}
		else{
			if (tutorial==3){
				output.append("  Below you can see all the men in your army seperated by unit type.\n  It is important to note that any unit that is killed in battle is permanently dead.\n");
				output.append("  If you win your men will automatically be healed up and ready to go.\n  If you retreat your men will NOT be healed, and will be more likely to die in your next battle.\n"+
					"  Now you are ready for a REAL battle.\n  Hit the Battle button after you look over your army to start the final part of the tutorial.\n\n");
				tutorial++;
			}
			output.append("  ******All Units in your Army:******\n\n");
			output.append("  ****Knights****\n");
			for(Map.Entry<String, Knight> element : soldiers.entrySet())
				output.append(""+element.getValue().toString());
			output.append("  ****Cavalry****\n");
			for(Map.Entry<String, Cavalry> element : cavalry.entrySet())
				output.append(""+element.getValue().toString());
			output.append("  ****Archers****\n");
			for(Map.Entry<String, Archer> element : archers.entrySet())
				output.append(""+element.getValue().toString());
			if(archers.isEmpty())
				output.append("\n");
		}
	}

	@Override
	public void buttonFourAction()   //////// BUY UNIT &&& RETREAT
	{
		if(inBattle){
			setParams();
			resetUnderMap();
			inBattle=false;
			turn.clear();
			resetBoard();
			connections.clear();
			output.setText("\n  You have retreated from battle!\n  Coward..\n\n");
			if (tutorial==2)
				tutorial++;
		}
		else{
			String[] temp={"Knight - 100g","Cavalry - 500g","Archer - 200g"};
			int choice = JOptionPane.showOptionDialog(null, 
     		 "Which Soldier would you like to buy?", 
     		 "Hire Soldier", 
     		 0, 
     		 JOptionPane.QUESTION_MESSAGE, 
     		 null, temp, null);
     		 
     		switch(choice){
     			case 0: if(cash>=100){
     				cash-=100;
     				data.setText(""+cash);
     				String name = findName();
     				soldiers.put(name,new Knight(name));
     				if(tutorial==1){
     					output.setText("  Good job!\n  You have bought your first Soldier.\n  You can view the stats of the new Soldier you have hired below.\n\n");
     					output.append("  ****New Knight Registration Form:****\n"+soldiers.get(name));
     					output.append("  Buy more units to increase the size of your army.\n  After that try going out and fighting savages for Gold, Fame, and Experience!\n  You can do this by clicking on the Train button at the bottom.\n\n");
     					tutorial++;
     				}
     				else
     					output.append("  ****New Knight Registration Form:****\n"+soldiers.get(name));
     			}
     			else{
     				JOptionPane.showMessageDialog(null,"You don't have enough Gold!","Your Gold: "+cash,JOptionPane.WARNING_MESSAGE);
     				buttonFourAction();
     			} 
     			break;
     			case 1: if(cash>=500){
     				cash-=500;
     				data.setText(""+cash);
     				String name = findName();
     				cavalry.put(name,new Cavalry(name));
     				if(tutorial==1){
     					output.setText("  Good job!\n  You have bought your first Soldier.\n  You can view the stats of the new Soldier you have hired below.\n\n");
     					output.append("  ****New Cavalry Registration Form:****\n"+cavalry.get(name));
     					output.append("  Buy more units to increase the size of your army.\n  After that try going out and fighting savages for Gold, Fame, and Experience!\n  You can do this by clicking on the Train button at the bottom.\n\n");
     					tutorial++;
     				}
     				else
     					output.append("  ****New Cavalry Registration Form:****\n"+cavalry.get(name));
     				
     			}
     			else{
     				JOptionPane.showMessageDialog(null,"You don't have enough Gold!","Your Gold: "+cash,JOptionPane.WARNING_MESSAGE);
     				buttonFourAction();
     			} 
	     			break;
	     		case 2: if(cash>=200){
	     			cash-=200;
	     			data.setText(""+cash);
	     			String name = findName();
	     			archers.put(name,new Archer(name));
	     			if(tutorial==1){
	     				output.setText("  Good job!\n  You have bought your first Soldier.\n  You can view the stats of the new Soldier you have hired below.\n\n");
	     				output.append("   ****New Archer Registration Form:****\n"+archers.get(name));
	     				output.append("  Buy more units to increase the size of your army.\n  After that try going out and fighting savages for Gold, Fame, and Experience!\n  You can do this by clicking on the Train button at the bottom.\n\n");
	     				tutorial++;
	     			}
	     			else
	     				output.append("   ****New Archer Registration Form:****\n"+archers.get(name));
	     			
	     		}
	     		else{
	     			JOptionPane.showMessageDialog(null,"You don't have enough Gold!","Your Gold: "+cash,JOptionPane.WARNING_MESSAGE);
	     			buttonFourAction();
	     		} 
	     			break;
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
		for(Map.Entry<String, Knight> element : soldiers.entrySet()){
			
			
			choice = JOptionPane.showOptionDialog(null, 
     		 "Where would you like to place your Knight?", 
     		 "Choose Location", 
     		 0, 
     		 JOptionPane.QUESTION_MESSAGE, 
     		 null, temp, null);
     		while(matrix[findRow(choice)][findCol(choice)]!=0)
				choice = JOptionPane.showOptionDialog(null, 
	     		 "You already have a unit there, place your Knight somewhere else!", 
	     		 "Choose Location", 
	     		 0, 
	     		 JOptionPane.QUESTION_MESSAGE, 
	     		 null, temp, null);
     		matrix[findRow(choice)][findCol(choice)] = 7;
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
		
		for(Map.Entry<String, Cavalry> element : cavalry.entrySet()){
			
			choice = JOptionPane.showOptionDialog(null, 
     		 "Where would you like to place your Cavalry?", 
     		 "Choose Location", 
     		 0, 
     		 JOptionPane.QUESTION_MESSAGE, 
     		 null, temp, null);
     		while(matrix[findRow(choice)][findCol(choice)]!=0)
				choice = JOptionPane.showOptionDialog(null, 
	     		 "You already have a unit there, place your Cavalry somewhere else!", 
	     		 "Choose Location", 
	     		 0, 
	     		 JOptionPane.QUESTION_MESSAGE, 
	     		 null, temp, null);
     		matrix[findRow(choice)][findCol(choice)] = 11; 
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
		
		for(Map.Entry<String, Archer> element : archers.entrySet()){
			
			choice = JOptionPane.showOptionDialog(null, 
     		 "Where would you like to place your Archer?", 
     		 "Choose Location", 
     		 0, 
     		 JOptionPane.QUESTION_MESSAGE, 
     		 null, temp, null);
     		while(matrix[findRow(choice)][findCol(choice)]!=0)
				choice = JOptionPane.showOptionDialog(null, 
	     		 "You already have a unit there, place your Archer somewhere else!", 
	     		 "Choose Location", 
	     		 0, 
	     		 JOptionPane.QUESTION_MESSAGE, 
	     		 null, temp, null);
     		matrix[findRow(choice)][findCol(choice)] = 9;
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
				if(matrix[row][col]!=7&&matrix[row][col]!=9&&matrix[row][col]!=11)
					matrix[row][col]=0;
	    }
	    Collections.shuffle(turn);
	//  System.out.println(turn);
	    board = matrix;
	    while(!turn.get(0).contains(" ")){
	   			Collections.rotate(turn,-1);
	   	}
	   	turn1=true;
	   	for(int x=0;x<turn.size();x++){
	   		endTurn();
	   	}
	   	turn1=false;
	   	for(int c=0;c<turn.size();c++){
	   			if(turn.get(c).contains(" "))
	   				goodAlive++;
	   			else
	   				badAlive++;
	   	}
		return matrix;
		
	}
	////////////////////////////////////////**IMPORTANT** FIND UNIT TYPE
	
	private String unitType(String n){
		if(soldiers.containsKey(n))
			return "[+K]";
		else
			if(cavalry.containsKey(n))
				return "[+C]";
			else
				if(archers.containsKey(n))
					return "[+A]";	
				else
					if(enemySavages.containsKey(n))
						return "[-S]";	
				else
					if(enemySoldiers.containsKey(n))
						return "[-K]";
				else
					if(enemyCavalry.containsKey(n))
						return "[-C]";
				else
					if(enemyArchers.containsKey(n))
						return "[-A]";
		return "";	
	}
	
	//////////////////////////////////////// BATTLE STUFF
	
	private ArrayList<String> inRange(){
		ArrayList<String> allEnemies = new ArrayList<String>();
		String name = turn.get(0);
		Point l = connections.get(name);
		int range = 0;
		boolean enemy = false;
		if(soldiers.containsKey(name))
			range = soldiers.get(name).getRange();
		else
			if(cavalry.containsKey(name))
				range = cavalry.get(name).getRange();
			else
				if(archers.containsKey(name))
					range = archers.get(name).getRange();
				else
					if(enemySavages.containsKey(name)){
						range = enemySavages.get(name).getRange();
						enemy = true;
					}
						
				else
					if(enemySoldiers.containsKey(name)){
						range = enemySoldiers.get(name).getRange();
						enemy = true;
					}
				else
					if(enemyCavalry.containsKey(name)){
						range = enemyCavalry.get(name).getRange();
						enemy = true;
					}
				else
					if(enemyArchers.containsKey(name)){
						range = enemyArchers.get(name).getRange();
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
	
/*	private String chooseAttack(ArrayList<String> enemies){
		String realChoice = "";
		try{
			int choice = JOptionPane.showOptionDialog(null, 
	     		 "Which enemy would you like to attack?", 
	     		 "Attack", 
	     		 0, 
	     		 JOptionPane.QUESTION_MESSAGE, 
	     		 null, enemies.toArray(), null);
	     	realChoice = enemies.get(choice);
		}catch(Exception e){}
		
     	
     	return realChoice;
	}
*/
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
		if((board[realX][realY]>0&&board[realX][realY]<7)||(board[realX][realY]>12))
			return true;
		return false;
	}
		
	private void attack(String n){
		int damage = 0;
		String name = turn.get(0);
		if(soldiers.containsKey(name))
			damage = soldiers.get(name).getDPS();
		else
			if(cavalry.containsKey(name))
				damage = cavalry.get(name).getDPS();
			else
				if(archers.containsKey(name))
					damage = archers.get(name).getDPS();
		
		showCurrentTurn();
					try{
						if(enemySavages.containsKey(n))
						if(enemySavages.get(n).damage(damage)){
							output.append(""+n+" took "+ damage+" damage!\n");
						}
						else{
							board[connections.get(n).x][connections.get(n).y] = 0;
							connections.remove(n);
							turn.remove(n);
							checkEnd();
							badAlive--;
							showCurrentTurn();
							
							output.append(""+n+" died..\n");
						}
				else
					if(enemySoldiers.containsKey(n))
						if(enemySoldiers.get(n).damage(damage)){
							output.append(""+n+" took "+ damage+" damage!\n");
						}
						else{
							board[connections.get(n).x][connections.get(n).y] = 0;
							connections.remove(n);
							turn.remove(n);
							checkEnd();
							badAlive--;
							showCurrentTurn();
							
							output.append(""+n+" died..\n");
						}
				else
					if(enemyCavalry.containsKey(n))
						if(enemyCavalry.get(n).damage(damage)){
							output.append(""+n+" took "+ damage+" damage!\n");
						}
						else{
							board[connections.get(n).x][connections.get(n).y] = 0;
							connections.remove(n);
							turn.remove(n);
							checkEnd();
							badAlive--;
							showCurrentTurn();
							
							output.append(""+n+" died..\n");
						}
				else
					if(enemyArchers.containsKey(n))
						if(enemyArchers.get(n).damage(damage)){
							output.append(""+n+" took "+ damage+" damage!\n");
						}
						else{
							board[connections.get(n).x][connections.get(n).y] = 0;
							connections.remove(n);
							turn.remove(n);
							checkEnd();
							badAlive--;
							showCurrentTurn();
							
							output.append(""+n+" died..\n");
						}
					}catch(Exception e){}
					
						endTurn();
		if(lose){
				resetBoard();
				setParams();
				lose = false;
				resetUnderMap();	
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
				for(Map.Entry<String, Archer> element : archers.entrySet()){
					element.getValue().heal();
				}
				for(Map.Entry<String, Knight> element : soldiers.entrySet()){
					element.getValue().heal();
				}
				for(Map.Entry<String, Cavalry> element : cavalry.entrySet()){
					element.getValue().heal();
				}
			}
			if(win){
				healUnits();
				resetBoard();
				win = false;
				setParams();
				resetUnderMap();
				inBattle=false;
				int gains = 300;
				cash+=gains;
				for(Map.Entry<String, Archer> element : archers.entrySet()){
					element.getValue().heal();
				}
				for(Map.Entry<String, Knight> element : soldiers.entrySet()){
					element.getValue().heal();
				}
				for(Map.Entry<String, Cavalry> element : cavalry.entrySet()){
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
				output.append("\n  You won the battle by killing all of your enemies!\n  You gained "+gains+" gold.\n\n");
				data.setText(""+cash);
					
			}			
			
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
	}
	
	////////////////////////////////////////MOVE
/*	private boolean moveUnit(){
		try{
			if(!turn.get(0).contains(" "))
				return true;
			String name = turn.get(0);	
			Point location = new Point();
			location = connections.get(name);
			String choice = chooseMove(location);
			board[location.x][location.y] = 0;
	     	switch(choice){
				case "Left": 
					location.y-=1;
					break;
				case "Forward": 
					location.x-=1;
					break;
				case "Backward": 
					location.x+=1;
					break;
				case "Right": 
					location.y+=1;
					break;
			}
			actions--;
			connections.put(name,location);
			updateUnderMap();
			repaint();
			showCurrentTurn();
		}catch(Exception e){}
		
		return false;
	}
*/	
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
			endTurn();
			showCurrentTurn();
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
			if(enemySavages.containsKey(name))
					damage = enemySavages.get(name).getDPS();
			else
				if(enemySoldiers.containsKey(name))
					damage = enemySoldiers.get(name).getDPS();
			else
				if(enemyCavalry.containsKey(name))
					damage = enemyCavalry.get(name).getDPS();
			else
				if(enemyArchers.containsKey(name))
					damage = enemyArchers.get(name).getDPS();
			if(soldiers.containsKey(n))
				if(soldiers.get(n).damage(damage)){
					output.append(""+n+" took "+ damage+" damage!\n");
				}
				else{
					board[connections.get(n).x][connections.get(n).y] = 0;
					connections.remove(n);
					turn.remove(n);
					soldiers.remove(n);
					goodAlive--;
					showCurrentTurn();
					checkEnd();
					
					output.append(""+n+" died..\n");
				}
			else
				if(cavalry.containsKey(n))
					if(cavalry.get(n).damage(damage)){
						output.append(""+n+" took "+ damage+" damage!\n");
					}
					else{
						board[connections.get(n).x][connections.get(n).y] = 0;
						connections.remove(n);
						turn.remove(n);
						cavalry.remove(n);
						checkEnd();
						goodAlive--;
						showCurrentTurn();
						
						
						output.append(""+n+" died..\n");
					}
				else
					if(archers.containsKey(n))
						if(archers.get(n).damage(damage)){
							output.append(""+n+" took "+ damage+" damage!\n");
						}
						else{
							board[connections.get(n).x][connections.get(n).y] = 0;
							connections.remove(n);
							turn.remove(n);
							archers.remove(n);
							checkEnd();
							goodAlive--;
							showCurrentTurn();
							
							output.append(""+n+" died..\n");
						}
					
			
		}
		
		return true;
	}
	
/*	private String chooseMove(Point l){
		ArrayList<String> temp = new ArrayList<String>();
		ArrayList<Point> allL = new ArrayList<Point>();
		for(Map.Entry<String, Point> element : connections.entrySet()){
			allL.add(element.getValue());
		}
//		System.out.println(allL);
		boolean clearLeft = true;
		boolean clearRight = true;
		boolean clearForward = true;
		boolean clearBackward = true;
		for(Point element:allL){
			if(element.x==l.x){
				if(element.y==l.y-1)
					clearLeft = false;
				if((element.y==l.y+1))
					clearRight = false;
			}
			if(element.y==l.y){
				if(element.x==l.x-1)
					clearForward = false;
				if((element.x==l.x+1))
					clearBackward = false;
			}
				
		}
		if(l.y-1>=0&&clearLeft){
			temp.add("Left");
		}
		if(l.x-1>=0&&clearForward){
			temp.add("Forward");
		}
		if(l.x+1<=14&&clearBackward){
			temp.add("Backward");
		}
		if(l.y+1<=8&&clearRight){
			temp.add("Right");
		}
		int choice = JOptionPane.showOptionDialog(null, 
     		 "Where would you like to move your Unit?", 
     		 "Move", 
     		 0, 
     		 JOptionPane.QUESTION_MESSAGE, 
     		 null, temp.toArray(), null);
     	String realChoice = temp.get(choice);
		return realChoice;	
	}
*/	
	////////////////////////////////////////TURN DATA BELOW
	private void endTurn(){
		if(soldiers.containsKey(turn.get(0)))
			board[connections.get(turn.get(0)).x][connections.get(turn.get(0)).y] = 7;
		else
			if(cavalry.containsKey(turn.get(0)))
				board[connections.get(turn.get(0)).x][connections.get(turn.get(0)).y] = 11; 
			else
				if(archers.containsKey(turn.get(0)))
					board[connections.get(turn.get(0)).x][connections.get(turn.get(0)).y] = 9;	
				else
					if(enemySavages.containsKey(turn.get(0)))
						board[connections.get(turn.get(0)).x][connections.get(turn.get(0)).y] = 13;	
				else
					if(enemySoldiers.containsKey(turn.get(0)))
						board[connections.get(turn.get(0)).x][connections.get(turn.get(0)).y] = 1;	
				else
					if(enemyCavalry.containsKey(turn.get(0)))
						board[connections.get(turn.get(0)).x][connections.get(turn.get(0)).y] = 5;	
				else
					if(enemyArchers.containsKey(turn.get(0)))
						board[connections.get(turn.get(0)).x][connections.get(turn.get(0)).y] = 3;	
					
		Collections.rotate(turn,-1);
		getActions();
		updateUnderMap();

		if(!turn.get(0).contains(" ")&&inBattle&&!turn1)
			moveAI();
		enemyMoved=false;
		enemyAttacked=false;
	}
	
	private void showCurrentTurn(){
		if(soldiers.containsKey(turn.get(0)))
			board[connections.get(turn.get(0)).x][connections.get(turn.get(0)).y] = 8;
		else
			if(cavalry.containsKey(turn.get(0)))
				board[connections.get(turn.get(0)).x][connections.get(turn.get(0)).y] = 12; 
			else
				if(archers.containsKey(turn.get(0)))
					board[connections.get(turn.get(0)).x][connections.get(turn.get(0)).y] = 10;	
				else
					if(enemyArchers.containsKey(turn.get(0)))
						board[connections.get(turn.get(0)).x][connections.get(turn.get(0)).y] = 4;	
					else
					if(enemySoldiers.containsKey(turn.get(0)))
						board[connections.get(turn.get(0)).x][connections.get(turn.get(0)).y] = 2;
					else
					if(enemyCavalry.containsKey(turn.get(0)))
						board[connections.get(turn.get(0)).x][connections.get(turn.get(0)).y] = 6; 
					else
					if(enemySavages.containsKey(turn.get(0)))
						board[connections.get(turn.get(0)).x][connections.get(turn.get(0)).y] = 14;	
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
		enemySoldiers = new HashMap<String,Knight>();
		enemyCavalry = new HashMap<String,Cavalry>();
		enemyArchers = new HashMap<String,Archer>();
		enemySavages = new HashMap<String,Savage>();
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
			enemySavages.put(temp,new Savage(temp));
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
			enemySoldiers.put(temp,new Knight(temp));
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
			enemyCavalry.put(temp,new Cavalry(temp));
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
			enemyArchers.put(temp,new Archer(temp));
			connections.put(temp,new Point(row,col));
			turn.add(temp);
		}
		board = matrix;
		return matrix;
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
		buttonThree.setText("View Army");
		buttonFour.setText("Hire a Soldier");
	//	input.setText("Test");
		data.setText(""+cash);
		dataLabel.setText("Gold:");
	}
	public void setBattleParams()
	{
		buttonOne.setText("EMPTY");
		buttonTwo.setText("EMPTY");
		buttonThree.setText("End Turn");
		buttonFour.setText("Retreat");
	//	input.setText("Test");
		data.setText(""+cash);
		dataLabel.setText("Gold:");
	}
	
	private void checkArmySize(){
		armySize= soldiers.size()+cavalry.size()+archers.size();
	}
	
	private void getActions(){
		String name = turn.get(0);
		switch(unitType(name)){
			case "[+K]": 
				actions = soldiers.get(name).getSpeed(); 
				break;
			case "[+C]": 
				actions = cavalry.get(name).getSpeed(); 
				break;
			case "[+A]": 
				actions = archers.get(name).getSpeed(); 
				break;
			case "[-S]": 
				actions = enemySavages.get(name).getSpeed(); 
				break;
			case "[-K]": 
				actions = enemySoldiers.get(name).getSpeed(); 
				break;
			case "[-C]": 
				actions = enemyCavalry.get(name).getSpeed(); 
				break;
			case "[-A]": 
				actions = enemyArchers.get(name).getSpeed(); 
				break;
		}
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
		for(Map.Entry<String, Knight> element : soldiers.entrySet()){
			element.getValue().heal();
		}
		for(Map.Entry<String, Cavalry> element : cavalry.entrySet()){
			element.getValue().heal();
		}
		for(Map.Entry<String, Archer> element : archers.entrySet()){
			element.getValue().heal();
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
		 int range =0;
		 if(soldiers.containsKey(name)){
		 	range = soldiers.get(name).getRange();
		 }
	   	 else
			if(cavalry.containsKey(name)){
				range = cavalry.get(name).getRange();
			}
			else
				if(archers.containsKey(name)){
					range = archers.get(name).getRange();
				}
					
		Point location = new Point();
			location = connections.get(name);
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
	public int goodAlive, badAlive;
	/////TESTING BELOW
	protected int os      = 50;	
	protected int numRows = 15;
	protected int numCols = 9;
	protected int width   = numCols*os+14;
	protected int height  = numRows*os+36;
	ImageIcon imageSource;
	ImageIcon imageSource1;
	ImageIcon imageSource2;
	ImageIcon imageSource3;
	int realX, realY;
	boolean inrange2;
	
	int[][] underMap = { 		  /*1*/   /*2*/   /*3*/   /*4*/   /*5*/   /*6*/   /*7*/   /*8*/   /*9*/
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
		imageSource = new ImageIcon( "grass2.jpg" );	
		imageSource1 = new ImageIcon( "grass21.jpg" );	
		imageSource2 = new ImageIcon( "grass22.jpg" );	
		imageSource3 = new ImageIcon( "grass23.jpg" );	
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
	  		realY = (xpos-8)/50;
	  		realX = (ypos-50)/50;
		//	System.out.println("X - "+realX);
		// 	System.out.println("Y - "+realY);
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
			JScrollPane scroll = new JScrollPane(output);
				scroll.setVerticalScrollBarPolicy(ScrollPaneConstants.VERTICAL_SCROLLBAR_ALWAYS);
				scroll.setHorizontalScrollBarPolicy(JScrollPane.HORIZONTAL_SCROLLBAR_NEVER);
			add(scroll);
			
			

			buttonOne = new JButton();
				buttonOne.setPreferredSize(new Dimension(120,25));
				buttonOne.addActionListener( new ActionOneListener() );
			add(buttonOne);

			buttonTwo = new JButton();
				buttonTwo.setPreferredSize(new Dimension(120,25));
				buttonTwo.addActionListener( new ActionTwoListener() );
			add(buttonTwo);

			buttonThree = new JButton();
				buttonThree.setPreferredSize(new Dimension(120,25));
				buttonThree.addActionListener( new ActionThreeListener() );
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
			super.paintComponent(g);

			for(int r=0; r<numRows; r++)
			  for(int c=0; c<numCols; c++){
			  	if(underMap[r][c]==0)
			  		g.drawImage(imageSource.getImage(),c*os,r*os,null);
			  	else
			  		if(underMap[r][c]==1)
				  		g.drawImage(imageSource1.getImage(),c*os,r*os,null);
				  	else
				  		if(underMap[r][c]==2)
					  		g.drawImage(imageSource2.getImage(),c*os,r*os,null);
					  	else
						  		g.drawImage(imageSource3.getImage(),c*os,r*os,null);
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
		
	}
	
	
	class ActionOneListener implements ActionListener {
		public void actionPerformed(ActionEvent event) {
			buttonOneAction();
	}}

	public void buttonOneAction()
	{}

	class ActionTwoListener implements ActionListener {
		public void actionPerformed(ActionEvent event) {
			buttonTwoAction();
	}}

	public void buttonTwoAction()
	{}

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
	
	public boolean checkAttack(){return false;}
	public void chooseAttack2(){
		
	}

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


////////////////////////////////////////UNIT CLASSES BELOW

class Knight
{
	private String name;
	private int hp;
	private int maxhp;
	private int maxDPS;
	private int minDPS;
	private int range;
	private int speed;
	
	public Knight(String n){
		name=n;
		maxhp=setMaxhp();
		hp=maxhp;
		maxDPS=setDPS();
		minDPS=50;
		range =1;
		speed =2;
	}
	
	public void heal(){
		hp = maxhp;
	}
	
	private int setMaxhp(){
		return (int)((Math.random()*101)+ 200);
	}
	
	private int setDPS(){
		return (int)((Math.random()*51)+ 75);
	}
	
	public String getName(){
		return name;
	}
	public int getHP(){
		return hp;
	}
	public int getRange(){
		return range;
	}
	public int getSpeed(){
		return speed;
	}
	public int getDPS(){
		return (int)((Math.random()*(maxDPS-minDPS+1))+ minDPS);
	}
	
	
	public boolean damage(int x){
		if(hp-x<=0)
			return false;
		else 
			hp-=x;
		return true;
	}
	
	@Override
	public String toString(){
		return "  Name: "+name+"\n  Current Health: "+hp+"\n  Damage: "+minDPS+" - "+maxDPS+"\n  Range: "+range+"\n  Speed: "+speed+"\n\n";
	}
}

class Cavalry
{
	private String name;
	private int hp;
	private int maxhp;
	private int maxDPS;
	private int minDPS;
	private int range;
	private int speed;
	
	public Cavalry(String n){
		name=n;
		maxhp=setMaxhp();
		hp=maxhp;
		maxDPS=setDPS();
		minDPS=150;
		range =2;
		speed =8;
	}
	
	public void heal(){
		hp = maxhp;
	}
	private int setMaxhp(){
		return (int)((Math.random()*250)+ 250);
	}
	
	private int setDPS(){
		return (int)((Math.random()*201)+ 200);
	}
	public String getName(){
		return name;
	}
	public int getHP(){
		return hp;
	}
	public int getRange(){
		return range;
	}
	public int getSpeed(){
		return speed;
	}
	public int getDPS(){
		return (int)((Math.random()*(maxDPS-minDPS+1))+ minDPS);
	}
	
	
	public boolean damage(int x){
		if(hp-x<=0)
			return false;
		else 
			hp-=x;
		return true;
	}
	
	@Override
	public String toString(){
		return "  Name: "+name+"\n  Current Health: "+hp+"\n  Damage: "+minDPS+" - "+maxDPS+"\n  Range: "+range+"\n  Speed: "+speed+"\n\n";
	}
}

class Archer
{
	private String name;
	private int hp;
	private int maxhp;
	private int maxDPS;
	private int minDPS;
	private int range;
	private int speed;
	
	public Archer(String n){
		name=n;
		maxhp=setMaxhp();
		hp=maxhp;
		maxDPS=setDPS();
		minDPS=25;
		range =8;
		speed =3;
	}
	
	public void heal(){
		hp = maxhp;
	}
	private int setMaxhp(){
		return (int)((Math.random()*51)+ 50);
	}
	
	private int setDPS(){
		return (int)((Math.random()*51)+ 100);
	}
	public String getName(){
		return name;
	}
	public int getHP(){
		return hp;
	}
	public int getRange(){
		return range;
	}
	public int getSpeed(){
		return speed;
	}
	public int getDPS(){
		return (int)((Math.random()*(maxDPS-minDPS+1))+ minDPS);
	}
	
	
	public boolean damage(int x){
		if(hp-x<=0)
			return false;
		else 
			hp-=x;
		return true;
	}
	
	@Override
	public String toString(){
		return "  Name: "+name+"\n  Current Health: "+hp+"\n  Damage: "+minDPS+" - "+maxDPS+"\n  Range: "+range+"\n  Speed: "+speed+"\n\n";
	}
}

class Savage
{
	private String name;
	private int hp;
	private int maxhp;
	private int maxDPS;
	private int minDPS;
	private int range;
	private int speed;
	
	public Savage(String n){
		name=n;
		maxhp=setMaxhp();
		hp=maxhp;
		maxDPS=setDPS();
		minDPS=30;
		range =1;
		speed =4;
	}
	
	public void heal(){
		hp = maxhp;
	}
	private int setMaxhp(){
		return (int)((Math.random()*31)+ 50);
	}
	
	private int setDPS(){
		return (int)((Math.random()*131)+ 30);
	}
	public String getName(){
		return name;
	}
	public int getHP(){
		return hp;
	}
	public int getRange(){
		return range;
	}
	public int getSpeed(){
		return speed;
	}
	public int getDPS(){
		return (int)((Math.random()*(maxDPS-minDPS+1))+ minDPS);
	}
	
	
	public boolean damage(int x){
		if(hp-x<=0)
			return false;
		else 
			hp-=x;
		return true;
	}
	
	@Override
	public String toString(){
		return "  Name: Savage"+"\n  Current Health: "+hp+"\n  Damage: "+minDPS+" - "+maxDPS+"\n  Range: "+range+"\n  Speed: "+speed+"\n\n";
	}
}
