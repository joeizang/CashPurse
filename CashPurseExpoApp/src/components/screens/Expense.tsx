import { StatusBar } from "expo-status-bar";
import { View, StyleSheet, Text } from 'react-native'

const styles = StyleSheet.create({
  container: {
    flex: 1,
    paddingTop: 40,
    paddingHorizontal: 20,
    backgroundColor: "#708090",
    justifyContent: 'space-between',
    paddingBottom: 10,
  },
  ctaSmallText: {
    color: '#fff',
    fontSize: 20, 
    fontWeight: '600',
  },
  recentExpense: {
    flex: 2,
    borderColor: "tan",
  }
});

export default function Expense() {
  return (
    <View>
      <View style={styles.container}>
        <View style={{ alignSelf: "flex-end" }}>
          <Text style={{ color: "#fff", fontSize: 20 }}>User Profile</Text>
        </View>
        <View>
          <Text style={{ fontSize: 35, fontWeight: "700", color: "#fff" }}>
            BudgetList Screen
          </Text>
        </View>
        <View
          style={{
            flexDirection: "row",
            rowGap: 10,
            justifyContent: "flex-start",
          }}
        >
          <Text
            style={[
              styles.ctaSmallText,
              { marginRight: 50, textAlign: "left" },
            ]}
          >
            30 Day Total :
          </Text>
          <Text
            style={[
              styles.ctaSmallText,
              { marginRight: 50, textAlign: "right" },
            ]}
          >
            XXX,XXX.XX
          </Text>
        </View>
        <View
          style={{
            flexDirection: "row",
            rowGap: 5,
            justifyContent: "flex-start",
            alignItems: "flex-start",
          }}
        >
          <Text style={[styles.ctaSmallText, { marginRight: 50 }]}>
            30 day Average
          </Text>
          <Text style={[styles.ctaSmallText, { textAlign: "right" }]}>
            XX,XXX.XX
          </Text>
        </View>
        <StatusBar style="dark" />
      </View>
      <View style={styles.recentExpense}>
        <View style={{ flex: 1 }}>
          <Text style={{ fontSize: 20, fontWeight: "500" }}>
            Last 5 recent items
          </Text>
        </View>
        <View style={{ flex: 3 }}>
          <Text>List of items</Text>
        </View>
        <View style={{ flex: 1 }}>
          <Text>Show More</Text>
        </View>
      </View>
    </View>
  );
}